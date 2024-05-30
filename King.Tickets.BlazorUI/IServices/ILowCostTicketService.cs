using King.Tickets.BlazorUI.Models;

namespace King.Tickets.BlazorUI.IServices;

public interface ILowCostTicketService
{
	Task<List<LowCostTicket>> GetLowCostTickets(TicketFilter ticketFilter);
}
