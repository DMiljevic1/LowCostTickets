namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class PricingOptions
{
	public List<string> FareType { get; set; }
	public bool IncludedCheckedBagsOnly { get; set; }
}
