using System.Text.Json.Serialization;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Departure
{
	[JsonPropertyName("iataCode")]
	public string IataCode { get; set; }

	[JsonPropertyName("terminal")]
	public string? Terminal { get; set; }

	[JsonPropertyName("at")]
	public DateTime At { get; set; }
}
