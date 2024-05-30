using King.Tickets.BlazorUI.Enums;
using King.Tickets.BlazorUI.IServices;
using King.Tickets.BlazorUI.Models;
using Microsoft.AspNetCore.Components;

namespace King.Tickets.BlazorUI.Pages.RazorPageBases;

public class LowCostTicketListBase : ComponentBase
{
	[Inject]
	public ILowCostTicketService _lowCostTicketService {  get; set; }
	protected List<LowCostTicket> lowCostTickets { get; set; }
	protected TicketFilter ticketFilter { get; set; }
	protected override async Task OnInitializedAsync()
	{
		lowCostTickets = new List<LowCostTicket>();
		ticketFilter = new TicketFilter();
	}
	protected async Task GetLowCostTickets()
	{
		lowCostTickets = await _lowCostTicketService.GetLowCostTickets(ticketFilter);
	}
}
