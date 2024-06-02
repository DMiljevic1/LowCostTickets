using King.Tickets.Application.DTOs;
using King.Tickets.Application.LowCostTickets.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace King.Tickets.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LowCostTicketController : ControllerBase
{
	private readonly IMediator _mediator;
	public LowCostTicketController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("GetLowCostTickets")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetLowCostTickets([FromQuery] TicketFilterDto ticketFilter, CancellationToken cancellationToken)
	{
        var lowCostTickets = await _mediator.Send(new GetLowCostTicketsCommand(ticketFilter), cancellationToken);
        return Ok(lowCostTickets);
    }
}
