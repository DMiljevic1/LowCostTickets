namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Amenity
{
	public string Description { get; set; }
	public bool IsChargeable { get; set; }
	public string AmenityType { get; set; }
	public AmenityProvider AmenityProvider { get; set; }
}
