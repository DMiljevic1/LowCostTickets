using King.Tickets.Application.Services.Integrations.AmadeusApi;
using MediatR;

namespace King.Tickets.Application.LowCostTickets.Commands;

public record GetLowCostTicketsCommand(TicketFilterDto TicketFilterDto) : IRequest<List<LowCostTicketDto>>;

public class GetLowCostTicketsHandler : IRequestHandler<GetLowCostTicketsCommand, List<LowCostTicketDto>>
{
	private readonly IAmadeusApiService _amadeusApi;
	public GetLowCostTicketsHandler(IAmadeusApiService amadeusApi)
	{
		_amadeusApi = amadeusApi;
	}
	public async Task<List<LowCostTicketDto>> Handle(GetLowCostTicketsCommand request, CancellationToken cancellationToken)
	{
		return await _amadeusApi.GetLowCostTickets(request.TicketFilterDto, cancellationToken);
	}
}
