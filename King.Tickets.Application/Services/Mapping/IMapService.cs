using King.Tickets.Application.DTOs;
using King.Tickets.Domain.Entities;

namespace King.Tickets.Application.Services.Mapping;

public interface IMapService
{
	List<LowCostTicket> MapToLowCostTickets(List<LowCostTicketDto> lowCostTicketsDto);
	List<LowCostTicketDto> MapToLowCostTicketsDto(List<LowCostTicket> lowCostTickets);
	TicketFilterHistory MapToTicketFilterHistory(TicketFilterDto ticketFilterDto);
}
