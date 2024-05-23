using King.Tickets.Domain.Enums;

namespace King.Tickets.Application.LowCostTickets.Commands;

public class LowCostTicketDto
{
    public string ArrivalAirport { get; set; }
    public string DepartureAirport { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int NumberOfPassengers { get; set; }
    public int NumberOfStops { get; set; }
    public string Currency { get; set; }
    public string TotalPrice { get; set; }
}
