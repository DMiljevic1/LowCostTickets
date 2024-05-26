using AutoMapper;
using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Enums;

namespace King.Tickets.Infrastructure.Services.Mapping.Profiles;

public class TicketFilterProfile : Profile
{
    public TicketFilterProfile()
    {
		CreateMap<TicketFilterHistory, TicketFilterDto>().
			ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.ToString()));
		CreateMap<TicketFilterDto, TicketFilterHistory>().
			ForMember(dest => dest.Currency, opt => opt.MapFrom(src => Enum.Parse<Currency>(src.Currency)));
	}
}
