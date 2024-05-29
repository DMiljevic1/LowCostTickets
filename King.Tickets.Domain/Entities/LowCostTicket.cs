using King.Tickets.Domain.Enums;

namespace King.Tickets.Domain.Entities;

public class LowCostTicket
{
    public int Id { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int NumberOfPassengers { get; set; }
    public int NumberOfTransfers { get; set; }
    public Currency Currency { get; set; }
    public double TotalPrice { get; set; }
    public int TicketFilterHistoryId { get; set; }
    public virtual TicketFilterHistory? TicketFilterHistory { get; set; }
}
