using King.Tickets.BlazorUI.Enums;
using System.Text.Json.Serialization;

namespace King.Tickets.BlazorUI.Models;

public class LowCostTicket
{
	[JsonPropertyName("departureAirport")]
	public string DepartureAirport { get; set; }

    [JsonPropertyName("arrivalAirport")]
    public string ArrivalAirport { get; set; }

    [JsonPropertyName("departureDate")]
    public DateTime DepartureDate { get; set; }

    [JsonPropertyName("returnDate")]
    public DateTime? ReturnDate { get; set; }

    [JsonPropertyName("numberOfPassengers")]
    public int NumberOfPassengers { get; set; }

    [JsonPropertyName("numberOfTransfers")]
    public int NumberOfTransfers { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("totalPrice")]
    public string TotalPrice { get; set; }
}
