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
	protected DateTime? departureDate { get; set; }
	protected DateTime currentDate { get; set; }
	protected DateTime minReturnDate { get; set; }
	protected bool isLoading = false;
	protected override async Task OnInitializedAsync()
	{
		lowCostTickets = new List<LowCostTicket>();
		ticketFilter = new TicketFilter();
		departureDate = DateTime.Today;
		currentDate = DateTime.Today;
	}
	protected async Task GetLowCostTickets()
	{
		if(isTicketFilterValid())
		{
			isLoading = true;
            ticketFilter.DepartureDate = departureDate!.Value;
            lowCostTickets = await _lowCostTicketService.GetLowCostTickets(ticketFilter);
			isLoading = false;
        }
	}
	private bool isTicketFilterValid()
	{
		if (ticketFilter.ArrivalAirport == null || ticketFilter.DepartureAirport == null ||
			ticketFilter.NumberOfPassengers == 0 || departureDate == null)
			return false;
		return true;
	}
}
