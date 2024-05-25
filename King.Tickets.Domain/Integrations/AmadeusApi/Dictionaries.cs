namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Dictionaries
{
	public Dictionary<string, Location> Locations { get; set; }
	public Dictionary<string, Aircraft> Aircraft { get; set; }
	public Dictionary<string, Currency> Currencies { get; set; }
	public Dictionary<string, Carrier> Carriers { get; set; }
}
