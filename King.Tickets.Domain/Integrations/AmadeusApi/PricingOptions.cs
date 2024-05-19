namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class PricingOptions
{
    public List<string> FareType { get; set; }
    public bool IncludedCheckedBangsOnly { get; set; }
}
