using King.Tickets.Application.Settings;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using King.Tickets.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;

public class AmadeusApiService : IAmadeusApiService
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly AmadeusApiSetting _amadeusApiSetting;
	private readonly IAmadeusApiAuthorizationService _amadeusApiAuthorizationService;
	private readonly ILogger<AmadeusApiService> _logger;
	public AmadeusApiService(IOptions<AmadeusApiSetting> amadeusApiSettings, 
		IAmadeusApiAuthorizationService amadeusApiAuthorizationService, ILogger<AmadeusApiService> logger, IHttpClientFactory httpClientFactory)
	{
		_amadeusApiSetting = amadeusApiSettings.Value;
		_amadeusApiAuthorizationService = amadeusApiAuthorizationService;
		_logger = logger;
		_httpClientFactory = httpClientFactory;
	}
	public async Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		var lowCostTickets = new List<LowCostTicketDto>();

		var queryParameters = GetQueryParameters(ticketFilterDto);
		var queryString = string.Join("&", queryParameters.Select(qp => $"{qp.Key}={Uri.EscapeDataString(qp.Value)}"));
		var amadeusApiEndpoint = _amadeusApiSetting.ApiTestPath + "?" + queryString;
		var responseContent = "";
		try
		{
			var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _amadeusApiAuthorizationService.GetAccessToken());
            using (var response = await httpClient.GetAsync(amadeusApiEndpoint, cancellationToken))
			{
                _logger.LogInformation($"Response from amadeus api: {response}");
                response.EnsureSuccessStatusCode();
				responseContent = await response.Content.ReadAsStringAsync();
                var amadeusApiResponse = JsonSerializer.Deserialize<AmadeusApiResponse>(responseContent);
                lowCostTickets = GenerateLowCostTickets(amadeusApiResponse!.FlightOffers, ticketFilterDto);
            }
		}
		catch (Exception e)
		{
			_logger.LogError($"Fail to deserialize response from amadeus api. Response: {responseContent}, Error: {e}");
			throw;
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
