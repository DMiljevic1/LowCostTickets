using System.Text.Json.Serialization;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;

public class AmadeusApiResponse
{
	[JsonPropertyName("data")]
	public List<FlightOffer> FlightOffers { get; set; }
}
