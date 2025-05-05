using Hotels.Domain.Common;
using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities;

public class Toilet : ApplicationNamedEntity
{
    public ICollection<Subobject> Subobjects { get; set; } = [];
}
