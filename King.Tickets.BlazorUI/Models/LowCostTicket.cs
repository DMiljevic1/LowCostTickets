using King.Tickets.BlazorUI.Enums;

namespace King.Tickets.BlazorUI.Models;

public class LowCostTicket
{
	public string DepartureAirport { get; set; }
	public string ArrivalAirport { get; set; }
	public DateTime DepartureDate { get; set; }
	public DateTime? ReturnDate { get; set; }
	public int NumberOfPassengers { get; set; }
	public int NumberOfTransfers { get; set; }
	public string Currency { get; set; }
	public string TotalPrice { get; set; }
}
