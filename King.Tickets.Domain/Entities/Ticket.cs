using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.Tickets.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string ArrivalAirport { get; set; }
        public string DepartureAirport { get; set; }
        public DateTime ArrivalDepartureDate { get; set; }
        public int NumberOfPassengers { get; set; }
        public double TotalPrice { get; set; }
    }
}
