using King.Tickets.Application.Settings;
using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Repositories;
using King.Tickets.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace King.Tickets.Infrastructure.Repositories;

public class TicketFilterHistoryRepository : ITicketFilterHistoryRepository
{
	private readonly TicketDbContext _ticketDbContext;
	private const int BatchSize = 100;
	private readonly ILogger<TicketFilterHistoryRepository> _logger;
	public TicketFilterHistoryRepository(TicketDbContext ticketDbContext, ILogger<TicketFilterHistoryRepository> logger)
	{
		_ticketDbContext = ticketDbContext;
		_logger = logger;
	}
	public async Task AddTicketFilterHistory(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		if(ticketFilterHistory != null)
		{
			ticketFilterHistory.CreationDate = DateTime.UtcNow;
			await _ticketDbContext.TicketFilterHistory.AddAsync(ticketFilterHistory, cancellationToken);
			await _ticketDbContext.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task<TicketFilterHistory?> GetTicketFilterHistory(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		if (ticketFilterHistory == null)
			return null;

		var ticketHistoryResult = await _ticketDbContext.TicketFilterHistory.Where(fh =>
			fh.ArrivalAirport == ticketFilterHistory.ArrivalAirport &&
			fh.DepartureAirport == ticketFilterHistory.DepartureAirport &&
			fh.DepartureDate == ticketFilterHistory.DepartureDate &&
			fh.NumberOfPassengers == ticketFilterHistory.NumberOfPassengers &&
			fh.Currency == ticketFilterHistory.Currency &&
			fh.ReturnDate == ticketFilterHistory.ReturnDate).Include(tfh => tfh.LowCostTickets).FirstOrDefaultAsync(cancellationToken);

		return ticketHistoryResult;
	}

	public async Task<List<TicketFilterHistory>> GetTicketFilterHistory(DateTime beforeDate)
    {
		return await _ticketDbContext.TicketFilterHistory.Where(tfh => tfh.CreationDate < beforeDate).ToListAsync();
	}

    public async Task DeleteTicketFilterHistory(List<TicketFilterHistory> ticketFilterHistory)
    {
        using var transaction = await _ticketDbContext.Database.BeginTransactionAsync();
        try
        {
            for (int i = 0; i < ticketFilterHistory.Count; i += BatchSize)
            {
                var batch = ticketFilterHistory.Skip(i).Take(BatchSize).ToList();
                _ticketDbContext.RemoveRange(batch);
                await _ticketDbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Ticket filter history cleaned");
        }
        catch (Exception e)
        {
            _logger.LogError("Deleting ticket filter history failed. Error: {@e}", e);
            await transaction.RollbackAsync();
            throw;
        }
    }
}
