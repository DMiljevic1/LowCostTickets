using System.Text.Json.Serialization;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Price
{
	[JsonPropertyName("currency")]
	public string Currency { get; set; }

	[JsonPropertyName("total")]
	public string Total { get; set; }

	[JsonPropertyName("base")]
	public string Base { get; set; }

	[JsonPropertyName("grandTotal")]
	public string GrandTotal { get; set; }
}
