namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class TravelerPricing
{
	public string TravelerId { get; set; }
	public string FareOption { get; set; }
	public string TravelerType { get; set; }
	public Price Price { get; set; }
	public List<FareDetailsBySegment> FareDetailsBySegment { get; set; }
}
