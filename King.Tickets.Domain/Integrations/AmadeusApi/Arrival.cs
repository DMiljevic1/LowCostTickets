namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Arrival
{
    public string IataCode { get; set; }
	public string? Terminal { get; set; }
	public DateTime At { get; set; }
}
