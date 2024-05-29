using FluentValidation;
using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Domain.Enums;

namespace King.Tickets.Infrastructure.Services.Validation;

public class TicketFilterValidator : AbstractValidator<TicketFilterDto>
{
	public TicketFilterValidator()
	{
		RuleFor(filter => filter.DepartureAirport).NotEmpty().WithMessage("Departure airport cannot be empty!");
		RuleFor(filter => filter.ArrivalAirport).NotEmpty().WithMessage("Arrival airport cannot be empty!");
		RuleFor(filter => filter.DepartureDate).NotEmpty().WithMessage("Departure date cannot be empty!");
		RuleFor(filter => filter.NumberOfPassengers).NotEmpty().WithMessage("Number of passengers cannot be empty!");
		RuleFor(filter => filter.Currency).Must(IsCurrencyValid).WithMessage("Invalid currency!");
		RuleFor(filter => filter.NumberOfPassengers).GreaterThan(0).WithMessage("Number of passengers must be greater than zero");
	}

	private bool IsCurrencyValid(Currency? currency)
	{
		if (!currency.HasValue)
			return true;
		foreach(var item in Enum.GetValues(typeof(Currency)))
		{
			if(item.Equals(currency.Value)) 
				return true;
		}

		return false;
	}
}
