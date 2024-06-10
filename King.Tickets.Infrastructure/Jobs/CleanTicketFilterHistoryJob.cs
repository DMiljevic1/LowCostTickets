using King.Tickets.Application.Jobs;
using King.Tickets.Application.Settings;
using King.Tickets.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace King.Tickets.Infrastructure.Jobs;

public class CleanTicketFilterHistoryJob : IJob
{
    private readonly ITicketFilterHistoryRepository _ticketFilterHistoryRepository;
    private readonly CleanTicketFilterHistoryJobSettings _settings;
    public CleanTicketFilterHistoryJob(ITicketFilterHistoryRepository ticketFilterHistoryRepository, IOptions<CleanTicketFilterHistoryJobSettings> settings)
    {
        _ticketFilterHistoryRepository = ticketFilterHistoryRepository;
        _settings = settings.Value;
    }

    public async Task Execute()
    {
        if (!_settings.Clean)
            return;
        var ticketFilterHistoryForDeletion = await _ticketFilterHistoryRepository.GetTicketFilterHistory(DateTime.UtcNow.AddDays(-(_settings.OlderThanInDays)));
        await _ticketFilterHistoryRepository.DeleteTicketFilterHistory(ticketFilterHistoryForDeletion);
    }
}
