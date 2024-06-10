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
using Hangfire;
using King.Tickets.Infrastructure.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace King.Tickets.API.Configuration;

public static class ServiceConfiguration
{
    private const string ConnectionString = "DefaultConnection";
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbConnection(services, configuration);
        ConfigureApplicationServices(services, configuration);
        ConfigureRepositories(services, configuration);
        ConfigureJobs(services, configuration);
    }
    private static void ConfigureDbConnection(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(ConnectionString)));
    }
    private static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AmadeusApiSetting>(configuration.GetSection("AmadeusApi"));
        services.Configure<CleanTicketFilterHistoryJobSettings>(configuration.GetSection("Jobs:CleanTicketFilterHistory"));
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
        services.AddTransient<CleanTicketFilterHistoryJob>();
    }
    private static void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITicketFilterHistoryRepository, TicketFilterHistoryRepository>();
        services.AddScoped<ILowCostTicketRepository, LowCostTicketRepository>();
    }
    private static void ConfigureJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetConnectionString(ConnectionString));
        });
        services.AddHangfireServer();
    }
    public static void ScheduleJobs(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();
        var serviceProvider = app.ApplicationServices;
        var recurringJobs = serviceProvider.GetRequiredService<IRecurringJobManager>();
        recurringJobs.AddOrUpdate<CleanTicketFilterHistoryJob>(
            "clean-ticket-filter-history-job",
            job => job.Execute(),
            Cron.Daily);
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
