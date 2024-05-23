namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Stop
{
    public string IataCode { get; set; }
    public string Duration { get; set; }
    public DateTime ArrivalAt { get; set; }
    public DateTime DepartureAt { get; set; }
}
