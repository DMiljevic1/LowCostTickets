using King.Tickets.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace King.Tickets.API.Configuration;

public static class MigrationConfiguration
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        IServiceScope scope = app.ApplicationServices.CreateScope();
        using TicketDbContext dbContext = scope.ServiceProvider.GetRequiredService<TicketDbContext>();
        dbContext.Database.Migrate();
    }
}
