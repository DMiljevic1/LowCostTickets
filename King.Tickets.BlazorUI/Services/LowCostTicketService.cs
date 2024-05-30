using King.Tickets.BlazorUI.IServices;
using King.Tickets.BlazorUI.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace King.Tickets.BlazorUI.Services;

public class LowCostTicketService : ILowCostTicketService
{
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _configuration;
	public LowCostTicketService(HttpClient httpClient, IConfiguration configuration)
	{
		_httpClient = httpClient;
		_configuration = configuration;
	}
	public async Task<List<LowCostTicket>> GetLowCostTickets(TicketFilter ticketFilter)
	{
        var lowCostTickets = new List<LowCostTicket>();
        var queryParameters = GetQueryParameters(ticketFilter);
        var queryString = string.Join("&", queryParameters.Select(qp => $"{qp.Key}={Uri.EscapeDataString(qp.Value)}"));
        var getLowCostTicketsEndpoint = _configuration["Endpoints:GetLowCostTickets"] + "?" + queryString;
        try
        {
            var response = await _httpClient.GetAsync(getLowCostTicketsEndpoint);
            string responseContent = await response.Content.ReadAsStringAsync();
            lowCostTickets = JsonSerializer.Deserialize<List<LowCostTicket>>(responseContent);
            if (lowCostTickets == null)
                return new List<LowCostTicket>();
        }
        catch (Exception e)
        {
            throw new HttpRequestException($"Request failed {e.Message}");
        }
        return lowCostTickets;
    }
    private Dictionary<string, string> GetQueryParameters(TicketFilter ticketFilterDto)
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
}
