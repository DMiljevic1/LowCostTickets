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
	private readonly ILogger<LowCostTicketService> _logger;
	public LowCostTicketService(ITicketFilterHistoryRepository ticketFilterHistoryRepository, IAmadeusApiService amadeusApiService, 
		ILowCostTicketRepository lowCostTicketRepository, IMapService mapService, ILogger<LowCostTicketService> logger)
	{
		_ticketFilterHistoryRepository = ticketFilterHistoryRepository;
		_amadeusApiService = amadeusApiService;
		_lowCostTicketRepository = lowCostTicketRepository;
		_mapService = mapService;
		_logger = logger;
	}

	public async Task<List<LowCostTicketDto>> GetLowCostTickets(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
		ticketFilterDto = AirportsToUpperCase(ticketFilterDto);
		var lowCostTicketsDto = new List<LowCostTicketDto>();
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
	private TicketFilterDto AirportsToUpperCase(TicketFilterDto ticketFilterDto)
	{
        ticketFilterDto.DepartureAirport = ticketFilterDto.DepartureAirport.ToUpper();
        ticketFilterDto.ArrivalAirport = ticketFilterDto.ArrivalAirport.ToUpper();

		return ticketFilterDto;
    }
	private async Task<TicketFilterHistory?> GetTicketFilterHistoryFromDb(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		var ticketFilterHistoryFromDb = await _ticketFilterHistoryRepository.GetTicketFilterHistory(ticketFilterHistory, cancellationToken);
		_logger.LogInformation($"Get ticket filter history from db: {ticketFilterHistoryFromDb}");
		return ticketFilterHistoryFromDb;
	}
	private async Task<List<LowCostTicketDto>> FetchLowCostTicketsFromAmadeusApi(TicketFilterHistory ticketFilterHistory,TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
	{
        var lowCostTicketsDto = await _amadeusApiService.GetLowCostTickets(ticketFilterDto, cancellationToken);
		_logger.LogInformation($"Successfully fetched low cost tickets from amadeus api: {lowCostTicketsDto}");
		return lowCostTicketsDto;
	}

	private async Task SaveTicketFilterHistory(TicketFilterHistory ticketFilterHistory, CancellationToken cancellationToken)
	{
		await _ticketFilterHistoryRepository.AddTicketFilterHistory(ticketFilterHistory, cancellationToken);
		_logger.LogDebug($"Saved ticket filter into database: {ticketFilterHistory}");
	}
	private async Task SaveLowCostTickets(int ticketFilterHistoryId, List<LowCostTicketDto> lowCostTicketsDto, CancellationToken cancellationToken)
	{
        var lowCostTickets = _mapService.MapToLowCostTickets(lowCostTicketsDto);
		_logger.LogInformation("Mapped lowCostTicketsDto to lowCostTickets.");
        lowCostTickets.ForEach(lct => lct.TicketFilterHistoryId = ticketFilterHistoryId);
        await _lowCostTicketRepository.AddLowCostTickets(lowCostTickets, cancellationToken);
    }
	private List<LowCostTicketDto> FetchLowCostTicketsFromLocalDatabase(TicketFilterHistory ticketFilterHistory)
	{
		var lowCostTicketsDto = _mapService.MapToLowCostTicketsDto(ticketFilterHistory.LowCostTickets.ToList());
		_logger.LogInformation($"Mapped lowCostTickets to lowCostTicketsDto: {lowCostTicketsDto}");
		return lowCostTicketsDto;
	}
}
