using King.Tickets.BlazorUI.Enums;

namespace King.Tickets.BlazorUI.Models;

public class TicketFilter
{
	public string DepartureAirport { get; set; }
	public string ArrivalAirport { get; set; }
	public DateTime DepartureDate { get; set; }
	public DateTime? ReturnDate { get; set; }
	public int NumberOfPassengers { get; set; }
	public Currency? Currency { get; set; }
}
