using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Repositories;
using King.Tickets.Infrastructure.DatabaseContext;

namespace King.Tickets.Infrastructure.Repositories;

public class LowCostTicketRepository : ILowCostTicketRepository
{
	private readonly TicketDbContext _ticketDbContext;
	private const int BatchSize = 100;
	public LowCostTicketRepository(TicketDbContext ticketDbContext)
	{
		_ticketDbContext = ticketDbContext;
	}

	public async Task AddLowCostTickets(List<LowCostTicket> lowCostTickets, CancellationToken cancellationToken)
	{
		using(var transaction = await _ticketDbContext.Database.BeginTransactionAsync(cancellationToken))
		{
			try
			{
                for (int i = 0; i < lowCostTickets.Count; i += BatchSize)
                {
                    var batch = lowCostTickets.Skip(i).Take(BatchSize).ToList();
                    await _ticketDbContext.AddRangeAsync(batch, cancellationToken);
                    await _ticketDbContext.SaveChangesAsync(cancellationToken);
                }

				await transaction.CommitAsync(cancellationToken);
            }
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				throw;
			}
		}
	}
}
