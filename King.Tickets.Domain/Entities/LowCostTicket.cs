﻿using King.Tickets.Domain.Enums;

namespace King.Tickets.Domain.Entities;

public class LowCostTicket
{
    public int Id { get; set; }
    public string ArrivalAirport { get; set; }
    public string DepartureAirport { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int NumberOfPassengers { get; set; }
    public int NumberOfStops { get; set; }
    public Currency Currency { get; set; }
    public double TotalPrice { get; set; }
}