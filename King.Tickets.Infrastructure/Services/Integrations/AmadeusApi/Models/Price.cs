using System.Text.Json.Serialization;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;

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
