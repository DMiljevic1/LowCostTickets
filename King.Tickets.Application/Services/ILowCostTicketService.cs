using King.Tickets.Application.DTOs;

namespace King.Tickets.Application.Services;

public interface ILowCostTicketService
{
	Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken);
}
