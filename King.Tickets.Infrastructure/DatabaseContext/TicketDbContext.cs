using King.Tickets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.Tickets.Infrastructure.DatabaseContext
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }
        public DbSet<LowCostTicket> LowCostTickets { get; set; }
    }
}
