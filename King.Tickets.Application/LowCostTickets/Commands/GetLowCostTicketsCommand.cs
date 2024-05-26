using King.Tickets.Application.Services;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using MediatR;

namespace King.Tickets.Application.LowCostTickets.Commands;

public record GetLowCostTicketsCommand(TicketFilterDto TicketFilterDto) : IRequest<List<LowCostTicketDto>>;

public class GetLowCostTicketsHandler : IRequestHandler<GetLowCostTicketsCommand, List<LowCostTicketDto>>
{
	private readonly ILowCostTicketService _lowCostTicketService;
	public GetLowCostTicketsHandler(ILowCostTicketService lowCostTicketService)
	{
		_lowCostTicketService = lowCostTicketService;
	}
	public async Task<List<LowCostTicketDto>> Handle(GetLowCostTicketsCommand request, CancellationToken cancellationToken)
	{
		return await _lowCostTicketService.GetLowCostTickets(request.TicketFilterDto, cancellationToken);
	}
}
