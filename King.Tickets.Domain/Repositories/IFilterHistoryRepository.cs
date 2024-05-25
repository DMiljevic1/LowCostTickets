using King.Tickets.Domain.Entities;

namespace King.Tickets.Domain.Repositories;

public interface IFilterHistoryRepository
{
	Task AddFilterHistory(FilterHistory filterHistory, CancellationToken cancellationToken);
}
