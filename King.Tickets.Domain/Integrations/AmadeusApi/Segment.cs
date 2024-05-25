using System.Text.Json.Serialization;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Segment
{
    [JsonPropertyName("departure")]
    public Departure Departure { get; set; }

	[JsonPropertyName("arrival")]
	public Arrival Arrival { get; set; }

	[JsonPropertyName("numberOfStops")]
	public int NumberOfStops { get; set; }
}
