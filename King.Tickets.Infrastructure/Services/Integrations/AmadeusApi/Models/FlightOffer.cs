using System.Text.Json.Serialization;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;

public class FlightOffer
{
	[JsonPropertyName("itineraries")]
	public List<Itinerary> Itineraries { get; set; }

	[JsonPropertyName("price")]
	public Price Price { get; set; }
}
