using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Repositories;
using King.Tickets.Infrastructure.DatabaseContext;

namespace King.Tickets.Infrastructure.Repositories;

public class FilterHistoryRepository : IFilterHistoryRepository
{
	private readonly TicketDbContext _ticketDbContext;
	public FilterHistoryRepository(TicketDbContext ticketDbContext)
	{
		_ticketDbContext = ticketDbContext;
	}
	public async Task AddFilterHistory(FilterHistory filterHistory, CancellationToken cancellationToken)
	{
		if(filterHistory != null)
		{
			await _ticketDbContext.FilterHistories.AddAsync(filterHistory, cancellationToken);
			await _ticketDbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
