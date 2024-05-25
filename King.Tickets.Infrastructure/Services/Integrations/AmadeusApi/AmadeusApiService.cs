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
	private readonly AmadeusApiAuthorizantionService _amadeusApiAuthorizationService;
	public AmadeusApiService(HttpClient httpClient, IOptions<AmadeusApiSetting> amadeusApiSettings, AmadeusApiAuthorizantionService amadeusApiAuthorizationService)
	{
		_httpClient = httpClient;
		_amadeusApiSetting = amadeusApiSettings.Value;
		_amadeusApiAuthorizationService = amadeusApiAuthorizationService;
	}
	public async Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		var lowCostTickets = new List<LowCostTicketDto>();
		_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _amadeusApiAuthorizationService.GetAccessToken());

		var queryParameters = GetQueryParameters(ticketFilterDto);
		var queryString = string.Join("&", queryParameters.Select(qp => $"{qp.Key}={Uri.EscapeDataString(qp.Value)}"));
		var amadeusApiEndpoint = _amadeusApiSetting.ApiTestPath + queryString;
		try
		{
			var response = await _httpClient.GetAsync(amadeusApiEndpoint, cancellationToken);
			string responseContent = await response.Content.ReadAsStringAsync();
			var amadeusApiResponse = JsonSerializer.Deserialize<AmadeusApiResponse>(responseContent);
			if (amadeusApiResponse is null || amadeusApiResponse.FlightOffers is null)
				return lowCostTickets;
			lowCostTickets = GenerateLowCostTickets(amadeusApiResponse.FlightOffers, ticketFilterDto);
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
	private List<LowCostTicketDto> GenerateLowCostTickets(List<FlightOffer> flightOffers, TicketFilterDto ticketFilterDto)
	{
		var lowCostTickets = new List<LowCostTicketDto>();
		foreach (var flightOffer in flightOffers)
		{
			foreach (var iternary in flightOffer.Itineraries)
			{
				foreach (var segment in iternary.Segments)
				{
					var lowCostTicket = GenerateLowCostTicket(flightOffer, segment, ticketFilterDto);
					lowCostTickets.Add(lowCostTicket);
				}
			}
		}

		return lowCostTickets;
	}

	private LowCostTicketDto GenerateLowCostTicket(FlightOffer flightOffer, Segment segment, TicketFilterDto ticketFilter)
	{
		var lowCostTicket = new LowCostTicketDto();
		lowCostTicket.ArrivalAirport = segment.Arrival.IataCode;
		lowCostTicket.DepartureAirport = segment.Departure.IataCode;
		lowCostTicket.NumberOfStops = segment.NumberOfStops;
		lowCostTicket.Currency = flightOffer.Price.Currency;
		lowCostTicket.TotalPrice = flightOffer.Price.GrandTotal;
		lowCostTicket.DepartureDate = ticketFilter.DepartureDate;
		lowCostTicket.ReturnDate = ticketFilter.ReturnDate;
		lowCostTicket.NumberOfPassengers = ticketFilter.NumberOfPassengers;

		return lowCostTicket;
	}
}
