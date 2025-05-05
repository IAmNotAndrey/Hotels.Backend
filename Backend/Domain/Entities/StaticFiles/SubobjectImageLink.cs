using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities.StaticFiles;

public class SubobjectImageLink : TitledImageLink
{
    public required Guid SubobjectId { get; set; }
    public Subobject Subobject { get; set; } = null!;
}
