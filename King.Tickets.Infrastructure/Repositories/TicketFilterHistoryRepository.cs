using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Repositories;
using King.Tickets.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace King.Tickets.Infrastructure.Repositories;

public class TicketFilterHistoryRepository : ITicketFilterHistoryRepository
{
	private readonly TicketDbContext _ticketDbContext;
	public TicketFilterHistoryRepository(TicketDbContext ticketDbContext)
	{
		_ticketDbContext = ticketDbContext;
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
}
