using AutoMapper;
using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Application.Services.Mapping;
using King.Tickets.Domain.Entities;

namespace King.Tickets.Infrastructure.Services.Mapping;

public class MapService : IMapService
{
	private readonly IMapper _mapper;
	public MapService(IMapper mapper)
	{
		_mapper = mapper;
	}
	public List<LowCostTicket> MapToLowCostTickets(List<LowCostTicketDto> lowCostTicketsDto)
	{
		return _mapper.Map<List<LowCostTicket>>(lowCostTicketsDto);
	}

	public List<LowCostTicketDto> MapToLowCostTicketsDto(List<LowCostTicket> lowCostTickets)
	{
		return _mapper.Map<List<LowCostTicketDto>>(lowCostTickets);
	}

	public TicketFilterHistory MapToTicketFilterHistory(TicketFilterDto ticketFilterDto)
	{
		return _mapper.Map<TicketFilterHistory>(ticketFilterDto);
	}
}
