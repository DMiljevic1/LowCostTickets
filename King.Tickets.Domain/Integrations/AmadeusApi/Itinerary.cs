using System.Text.Json.Serialization;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Itinerary
{
    [JsonPropertyName("segments")]
    public List<Segment> Segments { get; set; }
}
