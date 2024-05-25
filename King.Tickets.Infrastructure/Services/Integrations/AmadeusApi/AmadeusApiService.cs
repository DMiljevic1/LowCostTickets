using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Application.Settings;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Domain.Integrations.AmadeusApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using King.Tickets.Domain.Entities;
using Azure;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;

public class AmadeusApiService : IAmadeusApiService
{
	private readonly HttpClient _httpClient;
	private readonly AmadeusApiSetting _amadeusApiSetting;
	private readonly IMemoryCache _memoryCache;
	private const string AccessToken = "Access_Token";
	public AmadeusApiService(HttpClient httpClient, IOptions<AmadeusApiSetting> amadeusApiSettings, IMemoryCache memoryCache)
	{
		_httpClient = httpClient;
		_amadeusApiSetting = amadeusApiSettings.Value;
		_memoryCache = memoryCache;
	}
	public async Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		var lowCostTickets = new List<LowCostTicketDto>();
		_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAccessToken());

		var queryParameters = GetQueryParameters(ticketFilterDto);
		var queryString = string.Join("&", queryParameters.Select(qp => $"{qp.Key}={Uri.EscapeDataString(qp.Value)}"));
		var amadeusApiEndpoint = _amadeusApiSetting.ApiTestPath + queryString;
		try
		{
			var response = await _httpClient.GetAsync(amadeusApiEndpoint, cancellationToken);
			string responseContent = await response.Content.ReadAsStringAsync();
			var amadeusApiResponse = JsonSerializer.Deserialize<AmadeusApiResponse>(responseContent);
			if (amadeusApiResponse is null || amadeusApiResponse.Data is null)
				return lowCostTickets;
			lowCostTickets = GenerateLowCostTickets(amadeusApiResponse.Data, ticketFilterDto);
		}
		catch (Exception e)
		{
			throw new HttpRequestException($"Request failed {e.Message}");
		}
		return lowCostTickets;
	}

	private Dictionary<string, string> GetQueryParameters(TicketFilterDto ticketFilterDto)
	{
		var queryParameters = new Dictionary<string, string>
		{
			{ "originLocationCode", ticketFilterDto.ArrivalAirport},
			{ "destinationLocationCode", ticketFilterDto.DepartureAirport},
			{ "departureDate", ticketFilterDto.DepartureDate.ToString("yyyy-MM-dd")},
			{ "adults", ticketFilterDto.NumberOfPassengers.ToString()},
			{ "currencyCode", ticketFilterDto.Currency}
		};
		if (ticketFilterDto.ReturnDate != null)
		{
			queryParameters.Add("returnDate", ticketFilterDto.ReturnDate.Value.ToString("yyyy-MM-dd"));
		}

		return queryParameters;
	}
	private async Task<string> GetAccessToken()
	{
		if (!_memoryCache.TryGetValue(AccessToken, out string? accessToken))
		{
			accessToken = await RequestAccessToken();
		}

		return accessToken;
	}
	private async Task<string> RequestAccessToken()
	{
		var request = new HttpRequestMessage(HttpMethod.Post, _amadeusApiSetting.ApiAuthorizationPath);
		request.Content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("grant_type", "client_credentials"),
			new KeyValuePair<string, string>("client_id", _amadeusApiSetting.ApiKey),
			new KeyValuePair<string, string>("client_secret", _amadeusApiSetting.ApiSecret)
		});
		var response = await _httpClient.SendAsync(request);

		var token = await response.Content.ReadFromJsonAsync<AmadeusApiAuthorization>();
		if (token is not null)
		{
			_memoryCache.Set(AccessToken, token.Access_Token);
			return token.Access_Token;
		}

		return "";
	}
	private List<LowCostTicketDto> GenerateLowCostTickets(List<FlightOffer> flightOffers, TicketFilterDto ticketFilterDto)
	{
		var lowCostTickets = new List<LowCostTicketDto>();
		foreach (var flightOffer in flightOffers)
		{
			foreach (var iternary in flightOffer.Itineraries)
			{
				foreach (var segment in iternary.Segments)
				{
					var lowCostTicket = GenerateLowCostTicket(flightOffer, iternary, segment, ticketFilterDto);
					lowCostTickets.Add(lowCostTicket);
				}
			}
		}

		return lowCostTickets;
	}

	private LowCostTicketDto GenerateLowCostTicket(FlightOffer flightOffer, Itinerary itinerary, Segment segment, TicketFilterDto ticketFilter)
	{
		var lowCostTicket = new LowCostTicketDto();
		lowCostTicket.ArrivalAirport = segment.Arrival.IataCode;
		lowCostTicket.DepartureAirport = segment.Departure.IataCode;
		lowCostTicket.NumberOfStops = segment.NumberOfStops;
		lowCostTicket.Currency = flightOffer.Price.Currency;
		lowCostTicket.TotalPrice = flightOffer.Price.Total;
		lowCostTicket.DepartureDate = ticketFilter.DepartureDate;
		lowCostTicket.ReturnDate = ticketFilter.ReturnDate;

		return lowCostTicket;
	}
}
