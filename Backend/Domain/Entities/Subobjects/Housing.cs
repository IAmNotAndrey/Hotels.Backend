using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Entities.EntityTypes;

namespace Hotels.Domain.Entities.Subobjects;

/// <summary>
/// Жильё целиком
/// </summary>
public class Housing : Subobject, IHasType<HousingType>
{
    public override string TypeName { get; init; } = nameof(Housing);

    // ===

    public Guid? TypeId { get; set; }
    public HousingType? Type { get; set; }
}
