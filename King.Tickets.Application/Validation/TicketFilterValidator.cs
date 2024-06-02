using FluentValidation;
using King.Tickets.Application.DTOs;
using King.Tickets.Domain.Enums;

namespace King.Tickets.Application.Validation;

public class TicketFilterValidator : AbstractValidator<TicketFilterDto>
{
	private const int IataCodeLenght = 3;
	public TicketFilterValidator()
	{
		RuleFor(filter => filter.DepartureAirport).NotEmpty().WithMessage("Departure airport cannot be empty!");
		RuleFor(filter => filter.ArrivalAirport).NotEmpty().WithMessage("Arrival airport cannot be empty!");
		RuleFor(filter => filter.DepartureDate).NotEmpty().WithMessage("Departure date cannot be empty!");
		RuleFor(filter => filter.Currency).Must(IsCurrencyValid).WithMessage("Invalid currency!");
		RuleFor(filter => filter.NumberOfPassengers).GreaterThan(0).WithMessage("Number of passengers must be greater than zero!");
		RuleFor(filter => filter).Must(IsDepartureDateBeforeReturnDate).WithMessage("Departure date must be before return date!");
		RuleFor(filter => filter.DepartureDate).Must(IsValid).WithMessage("Invalid departure date!");
		RuleFor(filter => filter.DepartureAirport).Must(IsValidLenght).WithMessage("Departure airport code must be 3 characters long!");
        RuleFor(filter => filter.ArrivalAirport).Must(IsValidLenght).WithMessage("Arrival airport code must be 3 characters long!");
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
	private bool IsDepartureDateBeforeReturnDate(TicketFilterDto ticketFilterDto)
	{
		if(!ticketFilterDto.ReturnDate.HasValue)
			return true;
		return (ticketFilterDto.DepartureDate.Date < ticketFilterDto.ReturnDate.Value.Date) ? true : false;
	}
	private bool IsValid(DateTime departureDate)
	{
		return (departureDate.Date >= DateTime.Today)  ? true : false;
	}
	private bool IsValidLenght(string airportCode)
	{
		return (airportCode.Length == IataCodeLenght) ? true : false;
	}
}
