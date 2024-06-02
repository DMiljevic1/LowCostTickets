using King.Tickets.BlazorUI.IServices;
using King.Tickets.BlazorUI.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace King.Tickets.BlazorUI.Pages.RazorPageBases;

public class LowCostTicketListBase : ComponentBase
{
	[Inject]
	public ILowCostTicketService _lowCostTicketService {  get; set; }
    [Inject]
    protected IDialogService _dialogService { get; set; }
    protected List<LowCostTicket> lowCostTickets { get; set; }
	protected TicketFilter ticketFilter { get; set; }
	protected DateTime? departureDate { get; set; }
	protected DateTime currentDate { get; set; }
	protected DateTime minReturnDate { get; set; }
	protected bool isLoading = false;
	private const int IataCodeLenght = 3;
	protected override async Task OnInitializedAsync()
	{
		lowCostTickets = new List<LowCostTicket>();
		ticketFilter = new TicketFilter();
		currentDate = DateTime.Today;
	}
	protected async Task GetLowCostTickets()
	{
		var validationErrors = GetValidationErrors();
		if(validationErrors == string.Empty)
		{
			isLoading = true;
            ticketFilter.DepartureDate = departureDate!.Value;
            lowCostTickets = await _lowCostTicketService.GetLowCostTickets(ticketFilter);
			isLoading = false;
        }
		else
		{
			OpenErrorDialog(validationErrors);
		}
	}
	private string GetValidationErrors()
	{
		var validationErrors = "";
		if (ticketFilter.DepartureAirport == null)
			validationErrors = validationErrors + "Departure airport cannot be empty! ";
		else if(ticketFilter.DepartureAirport.Length != IataCodeLenght)
            validationErrors = validationErrors + "Departure airport must be 3 characters long! ";
        if (ticketFilter.ArrivalAirport == null)
            validationErrors = validationErrors + "Arrival airport cannot be empty! ";
		else if(ticketFilter.ArrivalAirport.Length != IataCodeLenght)
            validationErrors = validationErrors + "Arrival airport must be 3 characters long! ";
        if (departureDate == null)
            validationErrors = validationErrors + "Departure date cannot be empty! ";
		if(ticketFilter.NumberOfPassengers < 1)
            validationErrors = validationErrors + "Number of passengers must be greater then 0! ";

        return validationErrors;
	}

    protected void OpenErrorDialog(string errors)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameters = new DialogParameters();
        parameters.Add("ContentText", errors);
        _dialogService.Show<ErrorDialog>("Error", parameters, options);
    }
}
