using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities.EntityTypes;

public class HousingType : SubobjectType
{
    public ICollection<Housing> Housings { get; set; } = [];
}
