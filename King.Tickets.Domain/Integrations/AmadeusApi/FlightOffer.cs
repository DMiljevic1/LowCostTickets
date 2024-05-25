using System.Text.Json.Serialization;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class FlightOffer
{
	[JsonPropertyName("itineraries")]
	public List<Itinerary> Itineraries { get; set; }

	[JsonPropertyName("price")]
	public Price Price { get; set; }
}
