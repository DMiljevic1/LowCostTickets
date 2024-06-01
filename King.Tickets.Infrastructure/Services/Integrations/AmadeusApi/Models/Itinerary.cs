using System.Text.Json.Serialization;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;

public class Itinerary
{
    [JsonPropertyName("segments")]
    public List<Segment> Segments { get; set; }
}
