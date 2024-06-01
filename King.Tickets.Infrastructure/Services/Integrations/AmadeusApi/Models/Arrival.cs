using System.Text.Json.Serialization;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;

public class Arrival
{
	[JsonPropertyName("iataCode")]
	public string IataCode { get; set; }

	[JsonPropertyName("terminal")]
	public string? Terminal { get; set; }

	[JsonPropertyName("at")]
	public DateTime At { get; set; }
}
