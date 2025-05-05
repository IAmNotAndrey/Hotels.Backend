namespace Hotels.Domain.Entities.StaticFiles;

public class NearbyImageLink : StaticFile
{
    public required Guid NearbyId { get; set; }
    public Nearby Nearby { get; set; } = null!;
}
