namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class FlightOffer
{
	public string Type { get; set; }
	public string Id { get; set; }
	public string Source { get; set; }
	public bool InstantTicketingRequired { get; set; }
	public bool NonHomogeneous { get; set; }
	public bool OneWay { get; set; }
	public string LastTicketingDate { get; set; }
	public string LastTicketingDateTime { get; set; }
	public int NumberOfBookableSeats { get; set; }
	public List<Itinerary> Itineraries { get; set; }
	public Price Price { get; set; }
	public PricingOptions PricingOptions { get; set; }
	public List<string> ValidatingAirlineCodes { get; set; }
	public List<TravelerPricing> TravelerPricings { get; set; }
}
