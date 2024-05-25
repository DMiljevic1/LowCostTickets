namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class FareDetailsBySegment
{
	public string SegmentId { get; set; }
	public string Cabin { get; set; }
	public string FareBasis { get; set; }
	public string BrandedFare { get; set; }
	public string BrandedFareLabel { get; set; }
	public string Class { get; set; }
	public IncludedCheckedBags IncludedCheckedBags { get; set; }
	public List<Amenity> Amenities { get; set; }
}
