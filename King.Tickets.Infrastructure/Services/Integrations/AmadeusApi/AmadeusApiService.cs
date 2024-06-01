using King.Tickets.Application.Settings;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using King.Tickets.Application.DTOs;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;

public class AmadeusApiService : IAmadeusApiService
{
	private readonly HttpClient _httpClient;
	private readonly AmadeusApiSetting _amadeusApiSetting;
	private readonly IAmadeusApiAuthorizationService _amadeusApiAuthorizationService;
	public AmadeusApiService(HttpClient httpClient, IOptions<AmadeusApiSetting> amadeusApiSettings, IAmadeusApiAuthorizationService amadeusApiAuthorizationService)
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
		var amadeusApiEndpoint = _amadeusApiSetting.ApiTestPath + "?" + queryString;
		try
		{
			var response = await _httpClient.GetAsync(amadeusApiEndpoint, cancellationToken);
			string responseContent = await response.Content.ReadAsStringAsync();
			var amadeusApiResponse = JsonSerializer.Deserialize<AmadeusApiResponse>(responseContent);
			if (amadeusApiResponse == null || amadeusApiResponse.FlightOffers == null)
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
			{ "adults", ticketFilterDto.NumberOfPassengers.ToString()}
		};
		if (ticketFilterDto.ReturnDate != null)
		{
			queryParameters.Add("returnDate", ticketFilterDto.ReturnDate.Value.ToString("yyyy-MM-dd"));
		}
		if (ticketFilterDto.Currency != null)
		{
			queryParameters.Add("currencyCode", ticketFilterDto.Currency.Value.ToString());
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
				var lowCostTicket = GenerateLowCostTicket(flightOffer, ticketFilterDto);
				lowCostTicket.NumberOfTransfers = iternary.Segments.Count - 1;
				lowCostTickets.Add(lowCostTicket);
			}
		}

		return lowCostTickets;
	}

	private LowCostTicketDto GenerateLowCostTicket(FlightOffer flightOffer, TicketFilterDto ticketFilter)
	{
		var lowCostTicket = new LowCostTicketDto();
		lowCostTicket.DepartureAirport = ticketFilter.DepartureAirport;
		lowCostTicket.ArrivalAirport = ticketFilter.ArrivalAirport;
		lowCostTicket.Currency = flightOffer.Price.Currency;
		lowCostTicket.TotalPrice = flightOffer.Price.GrandTotal;
		lowCostTicket.DepartureDate = ticketFilter.DepartureDate;
		lowCostTicket.ReturnDate = ticketFilter.ReturnDate;
		lowCostTicket.NumberOfPassengers = ticketFilter.NumberOfPassengers;

		return lowCostTicket;
	}
}
