using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities.Comforts;

public class SubobjectComfort : Comfort
{
    public ICollection<Subobject> Subobjects { get; set; } = [];
}
