using King.Tickets.Application.DTOs;

namespace King.Tickets.Application.Services.Integrations.AmadeusApi;

public interface IAmadeusApiService
{
	Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken);
}
