﻿using System.Text.Json.Serialization;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;

public class Departure
{
	[JsonPropertyName("iataCode")]
	public string IataCode { get; set; }
}
