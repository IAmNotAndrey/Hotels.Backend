namespace Hotels.Domain.Entities.StaticFiles;

public class CafeImageLink : TitledImageLink
{
    public required Guid CafeId { get; set; }
    public Cafe Cafe { get; set; } = null!;
}
