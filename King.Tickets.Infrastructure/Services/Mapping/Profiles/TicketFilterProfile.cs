using AutoMapper;
using King.Tickets.Application.DTOs;
using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Enums;

namespace King.Tickets.Infrastructure.Services.Mapping.Profiles;

public class TicketFilterProfile : Profile
{
    public TicketFilterProfile()
    {
		CreateMap<TicketFilterHistory, TicketFilterDto>().ReverseMap();
	}
}
