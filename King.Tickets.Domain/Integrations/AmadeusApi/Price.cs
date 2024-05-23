namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Price
{
	public string Currency { get; set; }
	public string Total { get; set; }
	public string Base { get; set; }
	public List<Fee> Fees { get; set; }
	public string GrandTotal { get; set; }
	public List<AdditionalService>? AdditionalServices { get; set; }
}
