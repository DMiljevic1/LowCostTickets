using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Settings;
using King.Tickets.Infrastructure.DatabaseContext;
using King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;
using Microsoft.EntityFrameworkCore;

namespace King.Tickets.API.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbConnection(services, configuration);
        ConfigureApplicationServices(services, configuration);
    }
    private static void ConfigureDbConnection(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    private static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AmadeusApiSetting>(configuration.GetSection("AmadeusApi"));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetLowCostTicketsHandler).Assembly));
        services.AddMemoryCache();
        services.AddScoped<HttpClient>();
        services.AddScoped<IAmadeusApiService, AmadeusApiService>();
    }
}
