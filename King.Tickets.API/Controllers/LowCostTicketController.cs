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

	[HttpGet]
	public async Task<IActionResult> GetLowCostTickets([FromQuery] TicketFilterDto ticketFilter, CancellationToken cancellationToken)
	{
		try
		{
			var lowCostTickets = await _mediator.Send(new GetLowCostTicketsCommand(ticketFilter), cancellationToken);
			return Ok(lowCostTickets);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
