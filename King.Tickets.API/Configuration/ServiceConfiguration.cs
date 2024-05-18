using King.Tickets.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace King.Tickets.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDbConnection(services, configuration);
        }
        private static void ConfigureDbConnection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TicketDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultDatabase")));
        }
    }
}
