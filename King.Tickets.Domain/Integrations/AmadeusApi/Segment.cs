namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Segment
{
    public Departure Departure { get; set; }
    public Arrival Arrival { get; set; }
    public string CarrierCode { get; set; }
    public string Number { get; set; }
    public Aircraft Aircraft { get; set; }
    public Operating Operating { get; set; }
    public string Duration { get; set; }
    public string Id { get; set; }
    public int NumberOfStops { get; set; }
    public bool BlacklistedInEu { get; set; }
}
