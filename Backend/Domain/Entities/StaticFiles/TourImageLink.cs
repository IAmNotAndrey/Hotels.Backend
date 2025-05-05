namespace Hotels.Domain.Entities.StaticFiles;

public class TourImageLink : TitledImageLink
{
    public required Guid TourId { get; set; }
    public Tour Tour { get; set; } = null!;
}
