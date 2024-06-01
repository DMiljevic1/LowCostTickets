﻿using AutoMapper;
using FluentValidation;
using King.Tickets.Application.DTOs;
using King.Tickets.Application.Services;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Services.Mapping;
using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Repositories;

namespace King.Tickets.Infrastructure.Services;

public class LowCostTicketService : ILowCostTicketService
{
	private readonly ITicketFilterHistoryRepository _ticketFilterHistoryRepository;
	private readonly IAmadeusApiService _amadeusApiService;
	private readonly ILowCostTicketRepository _lowCostTicketRepository;
	private readonly IMapService _mapService;
	private readonly IValidator<TicketFilterDto> _validator;
	public LowCostTicketService(ITicketFilterHistoryRepository ticketFilterHistoryRepository, IAmadeusApiService amadeusApiService, 
		ILowCostTicketRepository lowCostTicketRepository, IMapService mapService, IValidator<TicketFilterDto> validator)
	{
		_ticketFilterHistoryRepository = ticketFilterHistoryRepository;
		_amadeusApiService = amadeusApiService;
		_lowCostTicketRepository = lowCostTicketRepository;
		_mapService = mapService;
		_validator = validator;
	}

	public async Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		var lowCostTicketsDto = new List<LowCostTicketDto>();
		var validationResults = _validator.Validate(ticketFilterDto);
		if (!validationResults.IsValid)
			throw new ValidationException("Validation failed. Errors: " + validationResults);

		var ticketFilterHistory = _mapService.MapToTicketFilterHistory(ticketFilterDto);
		var ticketFilterHistoryFromDb = await GetTicketFilterHistoryFromDb(ticketFilterHistory, cancellationToken);

		if (ticketFilterHistoryFromDb == null)
		{
			lowCostTicketsDto = await FetchLowCostTicketsFromAmadeusApi(ticketFilterHistory, ticketFilterDto, cancellationToken);
			await SaveLowCostTickets(ticketFilterHistory.Id, lowCostTicketsDto, cancellationToken);
		}
		else
			lowCostTicketsDto = FetchLowCostTicketsFromLocalDatabase(ticketFilterHistoryFromDb);

		return lowCostTicketsDto;
	}
	private async Task<TicketFilterHistory?> GetTicketFilterHistoryFromDb(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		return await _ticketFilterHistoryRepository.GetTicketFilterHistory(ticketFilterHistory, cancellationToken);
	}
	private async Task<List<LowCostTicketDto>> FetchLowCostTicketsFromAmadeusApi(TicketFilterHistory ticketFilterHistory,TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		await _ticketFilterHistoryRepository.AddTicketFilterHistory(ticketFilterHistory, cancellationToken);
		return await _amadeusApiService.GetLowCostTickets(ticketFilterDto, cancellationToken);
	}
	private async Task SaveLowCostTickets(int ticketFilterHistoryId, List<LowCostTicketDto> lowCostTicketsDto, CancellationToken cancellationToken)
	{
        var lowCostTickets = _mapService.MapToLowCostTickets(lowCostTicketsDto);
        lowCostTickets.ForEach(lct => lct.TicketFilterHistoryId = ticketFilterHistoryId);
        await _lowCostTicketRepository.AddLowCostTickets(lowCostTickets, cancellationToken);
    }
	private List<LowCostTicketDto> FetchLowCostTicketsFromLocalDatabase(TicketFilterHistory ticketFilterHistory)
	{
		return _mapService.MapToLowCostTicketsDto(ticketFilterHistory.LowCostTickets.ToList());
	}
}
