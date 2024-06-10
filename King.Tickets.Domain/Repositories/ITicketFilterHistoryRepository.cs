using King.Tickets.Domain.Entities;

namespace King.Tickets.Domain.Repositories;

public interface ITicketFilterHistoryRepository
{
	Task AddTicketFilterHistory(TicketFilterHistory filterHistory, CancellationToken cancellationToken);
	Task<TicketFilterHistory?> GetTicketFilterHistory(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken);
	Task<List<TicketFilterHistory>> GetTicketFilterHistory(DateTime dateTime);
	Task DeleteTicketFilterHistory(List<TicketFilterHistory> ticketFilterHistory);
}
