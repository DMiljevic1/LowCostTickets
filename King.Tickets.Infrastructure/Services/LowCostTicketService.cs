using AutoMapper;
using FluentValidation;
using King.Tickets.Application.DTOs;
using King.Tickets.Application.Services;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Services.Mapping;
using King.Tickets.Domain.Entities;
using King.Tickets.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace King.Tickets.Infrastructure.Services;

public class LowCostTicketService : ILowCostTicketService
{
	private readonly ITicketFilterHistoryRepository _ticketFilterHistoryRepository;
	private readonly IAmadeusApiService _amadeusApiService;
	private readonly ILowCostTicketRepository _lowCostTicketRepository;
	private readonly IMapService _mapService;
	private readonly IValidator<TicketFilterDto> _validator;
	private readonly ILogger<LowCostTicketService> _logger;
	public LowCostTicketService(ITicketFilterHistoryRepository ticketFilterHistoryRepository, IAmadeusApiService amadeusApiService, 
		ILowCostTicketRepository lowCostTicketRepository, IMapService mapService, IValidator<TicketFilterDto> validator, 
		ILogger<LowCostTicketService> logger)
	{
		_ticketFilterHistoryRepository = ticketFilterHistoryRepository;
		_amadeusApiService = amadeusApiService;
		_lowCostTicketRepository = lowCostTicketRepository;
		_mapService = mapService;
		_validator = validator;
		_logger = logger;
	}

	public async Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		var lowCostTicketsDto = new List<LowCostTicketDto>();
		var validationResults = _validator.Validate(ticketFilterDto);
		if (!validationResults.IsValid)
		{
			_logger.LogError("Validation failed: {@validationResults}", validationResults);
			throw new ValidationException("Validation failed. Errors: " + validationResults);
		}

		var ticketFilterHistory = _mapService.MapToTicketFilterHistory(ticketFilterDto);
		var ticketFilterHistoryFromDb = await GetTicketFilterHistoryFromDb(ticketFilterHistory, cancellationToken);

		if (ticketFilterHistoryFromDb == null)
		{
			await SaveTicketFilterHistory(ticketFilterHistory, cancellationToken);
			lowCostTicketsDto = await FetchLowCostTicketsFromAmadeusApi(ticketFilterHistory, ticketFilterDto, cancellationToken);
			await SaveLowCostTickets(ticketFilterHistory.Id, lowCostTicketsDto, cancellationToken);
		}
		else
			lowCostTicketsDto = FetchLowCostTicketsFromLocalDatabase(ticketFilterHistoryFromDb);

		return lowCostTicketsDto;
	}
	private async Task<TicketFilterHistory?> GetTicketFilterHistoryFromDb(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		try
		{
			var ticketFilterHistoryFromDb = await _ticketFilterHistoryRepository.GetTicketFilterHistory(ticketFilterHistory, cancellationToken);
			_logger.LogDebug("Get ticket filter history from db: {@ticketFilterHistoryFromDb}", ticketFilterHistoryFromDb);
			return ticketFilterHistoryFromDb;
        }
		catch (Exception e)
		{
			_logger.LogError("Get ticketFilterHistory from db failed: {@e}", e);
			throw;
		}
	}
	private async Task<List<LowCostTicketDto>> FetchLowCostTicketsFromAmadeusApi(TicketFilterHistory ticketFilterHistory,TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		var lowCostTicketsDto = new List<LowCostTicketDto>();
		try
		{
            lowCostTicketsDto = await _amadeusApiService.GetLowCostTickets(ticketFilterDto, cancellationToken);
			_logger.LogDebug("Successfully fetched low cost tickets from amadeus api: {@lowCostTicketsDto}", lowCostTicketsDto);
			return lowCostTicketsDto;
        }
		catch (Exception e)
		{
			_logger.LogError("An error occured while fetching low cost tickets from amadeus api: {@e}", e);
			throw;
		}
	}

	private async Task SaveTicketFilterHistory(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		try
		{
            await _ticketFilterHistoryRepository.AddTicketFilterHistory(ticketFilterHistory, cancellationToken);
            _logger.LogDebug("Saved ticket filter into database: {@ticketFilterHistory}", ticketFilterHistory);
        }
		catch (Exception e)
		{
            _logger.LogError("An error occured while saving ticketFilterHistory: {@ticketFilterHistory}, " + 
				"{@e}", ticketFilterHistory, e);
            throw;
		}
	}
	private async Task SaveLowCostTickets(int ticketFilterHistoryId, List<LowCostTicketDto> lowCostTicketsDto, CancellationToken cancellationToken)
	{
		var lowCostTickets = new List<LowCostTicket>();
		try
		{
            lowCostTickets = _mapService.MapToLowCostTickets(lowCostTicketsDto);
			_logger.LogDebug("Mapped lowCostTicketsDto to lowCostTickets.");
            lowCostTickets.ForEach(lct => lct.TicketFilterHistoryId = ticketFilterHistoryId);
            await _lowCostTicketRepository.AddLowCostTickets(lowCostTickets, cancellationToken);
        }
		catch (Exception e)
		{
			_logger.LogError("An error occured while mapping and saving: {@lowCostTicketsDto}, " +
				"{@lowCostTickets}, {@e}", lowCostTickets, lowCostTicketsDto, e);
			throw;
		}
    }
	private List<LowCostTicketDto> FetchLowCostTicketsFromLocalDatabase(TicketFilterHistory ticketFilterHistory)
	{
		try
		{
			var lowCostTicketsDto = _mapService.MapToLowCostTicketsDto(ticketFilterHistory.LowCostTickets.ToList());
			_logger.LogDebug("Mapped lowCostTickets to lowCostTicketsDto: {@lowCostTicketsDto}", lowCostTicketsDto);
			return lowCostTicketsDto;
        }
		catch (Exception e)
		{
			_logger.LogError("Mapping lowCostTickets to lowCostTicketsDto failed: {@e}", e);
			throw;
		}
	}
}
