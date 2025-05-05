using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.StaticFiles;

public class ObjectImageLink : TitledImageLink
{
    public required string ApplicationObjectId { get; set; }
    public ApplicationObject ApplicationObject { get; set; } = null!;
}
