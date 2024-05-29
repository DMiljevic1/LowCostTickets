using King.Tickets.Domain.Enums;

namespace King.Tickets.Application.LowCostTickets.Commands;

public class TicketFilterDto
{
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int NumberOfPassengers { get; set; }
    public Currency? Currency { get; set; }
}
