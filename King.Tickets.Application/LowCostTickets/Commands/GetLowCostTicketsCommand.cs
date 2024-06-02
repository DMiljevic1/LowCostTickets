using FluentValidation;
using King.Tickets.Application.DTOs;
using King.Tickets.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace King.Tickets.Application.LowCostTickets.Commands;

public record GetLowCostTicketsCommand(TicketFilterDto TicketFilterDto) : IRequest<List<LowCostTicketDto>>;

public class GetLowCostTicketsHandler : IRequestHandler<GetLowCostTicketsCommand, List<LowCostTicketDto>>
{
	private readonly ILowCostTicketService _lowCostTicketService;
	private readonly IValidator<TicketFilterDto> _validator;
	private readonly ILogger<GetLowCostTicketsHandler> _logger;
	public GetLowCostTicketsHandler(ILowCostTicketService lowCostTicketService, IValidator<TicketFilterDto> validator, 
		ILogger<GetLowCostTicketsHandler> logger)
	{
		_lowCostTicketService = lowCostTicketService;
		_validator = validator;
		_logger = logger;
	}
	public async Task<List<LowCostTicketDto>> Handle(GetLowCostTicketsCommand request, CancellationToken cancellationToken)
	{
        var validationResults = _validator.Validate(request.TicketFilterDto);
        if (!validationResults.IsValid)
        {
            _logger.LogError("Validation failed: {@validationResults}", validationResults);
            throw new ValidationException("Validation failed. Errors: " + validationResults);
        }
        return await _lowCostTicketService.GetLowCostTickets(request.TicketFilterDto, cancellationToken);
	}
}
