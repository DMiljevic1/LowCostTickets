using System.Text.Json.Serialization;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class AmadeusApiResponse
{
	[JsonPropertyName("data")]
	public List<FlightOffer> FlightOffers { get; set; }
}
