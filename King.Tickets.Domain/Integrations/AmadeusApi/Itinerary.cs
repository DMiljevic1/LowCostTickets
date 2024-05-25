namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Itinerary
{
    public string Duration { get; set; }
    public List<Segment> Segments { get; set; }
}
