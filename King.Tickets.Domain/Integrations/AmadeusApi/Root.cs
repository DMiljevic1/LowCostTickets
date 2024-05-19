namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Root
{
	public Meta Meta { get; set; }
	public List<FlightOffer> Data { get; set; }
	public Dictionaries Dictionaries { get; set; }
}
