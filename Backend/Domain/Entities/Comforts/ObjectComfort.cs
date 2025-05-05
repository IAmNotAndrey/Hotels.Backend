using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Comforts;

public class ObjectComfort : Comfort
{
    public ICollection<Partner> Partners { get; set; } = [];
}
