using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.EntityTypes;

public class ObjectType : TypeEntity
{
    public ICollection<Partner> Partners { get; set; } = [];
}
