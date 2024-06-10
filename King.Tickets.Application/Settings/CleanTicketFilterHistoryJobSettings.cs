namespace King.Tickets.Application.Settings;

public class CleanTicketFilterHistoryJobSettings
{
    public bool Clean { get; set; }
    public int OlderThanInDays { get; set; }
}
