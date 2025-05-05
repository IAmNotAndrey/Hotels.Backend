using Hotels.Domain.Entities.EntityTypes;

namespace Hotels.Domain.Common.Interfaces;

public interface IHasType<T> where T : TypeEntity
{
    Guid? TypeId { get; set; }
    T? Type { get; set; }
}
