using AutoMapper;
using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Enums;

namespace King.Tickets.Infrastructure.Services.Mapping.Profiles;

public class LowCostTicketProfile : Profile
{
    public LowCostTicketProfile()
    {
        CreateMap<LowCostTicket, LowCostTicketDto>().
            ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.ToString())).
            ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice.ToString()));
        CreateMap<LowCostTicketDto, LowCostTicket>().
            ForMember(dest => dest.Currency, opt => opt.MapFrom(src => Enum.Parse<Currency>(src.Currency))).
            ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => double.Parse(src.TotalPrice)));
    }
}
