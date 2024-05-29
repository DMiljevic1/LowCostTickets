using King.Tickets.Domain.Enums;

namespace King.Tickets.Domain.Entities;

public class TicketFilterHistory
{
    public int Id { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
	public DateTime DepartureDate { get; set; }
	public DateTime? ReturnDate { get; set; }
	public int NumberOfPassengers { get; set; }
	public Currency Currency { get; set; }
	public virtual ICollection<LowCostTicket> LowCostTickets { get; set; } = new List<LowCostTicket>();
}
