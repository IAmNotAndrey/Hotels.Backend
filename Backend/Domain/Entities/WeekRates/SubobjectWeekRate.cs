using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities.WeekRates;

public class SubobjectWeekRate : WeekRate
{
    public required Guid SubobjectId { get; set; }
    public Subobject Subobject { get; set; } = null!;
}
