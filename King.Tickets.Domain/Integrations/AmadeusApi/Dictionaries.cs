namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Dictionaries
{
	public Dictionary<string, Location> Locations { get; set; }
	public Dictionary<string, string> Aircraft { get; set; }
	public Dictionary<string, string> Currencies { get; set; }
	public Dictionary<string, string> Carriers { get; set; }
}
