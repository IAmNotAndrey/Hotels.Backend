namespace Hotels.Domain.Entities.StaticFiles;

public class CafeMenuFileLink : StaticFile
{
    public required Guid CafeId { get; set; }
    public Cafe Cafe { get; set; } = null!;
}
