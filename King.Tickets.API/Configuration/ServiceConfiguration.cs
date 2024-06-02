using AutoMapper;
using FluentValidation;
using King.Tickets.API.ExceptionHandlers;
using King.Tickets.Application.DTOs;
using King.Tickets.Application.LowCostTickets.Commands;
using King.Tickets.Application.Services;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Services.Mapping;
using King.Tickets.Application.Settings;
using King.Tickets.Domain.Repositories;
using King.Tickets.Infrastructure.DatabaseContext;
using King.Tickets.Infrastructure.Repositories;
using King.Tickets.Infrastructure.Services;
using King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;
using King.Tickets.Infrastructure.Services.Mapping;
using King.Tickets.Infrastructure.Services.Mapping.Profiles;
using King.Tickets.Application.Validation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace King.Tickets.API.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbConnection(services, configuration);
        ConfigureApplicationServices(services, configuration);
        ConfigureRepositories(services, configuration);
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
        services.AddHttpClient();
        services.AddScoped<IAmadeusApiService, AmadeusApiService>();
        services.AddScoped<IAmadeusApiAuthorizationService, AmadeusApiAuthorizationService>();
        services.AddScoped<ILowCostTicketService, LowCostTicketService>();
        services.AddScoped<IMapService, MapService>();
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new LowCostTicketProfile());
            mc.AddProfile(new TicketFilterProfile());
        });
        services.AddSingleton(mapperConfig.CreateMapper());
        services.AddScoped<IValidator<TicketFilterDto>, TicketFilterValidator>();
    }
    private static void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITicketFilterHistoryRepository, TicketFilterHistoryRepository>();
        services.AddScoped<ILowCostTicketRepository, LowCostTicketRepository>();
    }
    public static void ConfigureLogging(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
    }
    public static void ConfigureExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}
