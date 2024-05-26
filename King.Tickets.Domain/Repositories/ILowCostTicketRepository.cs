using King.Tickets.Domain.Entities;

namespace King.Tickets.Domain.Repositories;

public interface ILowCostTicketRepository
{
	Task AddLowCostTickets(List<LowCostTicket> lowCostTickets, CancellationToken cancellationToken);
}
