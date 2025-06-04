using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ISubobjectRepo
{
    Task<Subobject> GetSubobjectWithBookingsAsync(Guid subobjectId);
}
