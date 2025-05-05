namespace Hotels.Domain.Entities.StaticFiles;

public class AttractionImageLink : TitledImageLink
{
    public required Guid AttractionId { get; set; }
    public Attraction Attraction { get; set; } = null!;
}
