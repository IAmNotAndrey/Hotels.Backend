using Hotels.Application.Exceptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ISubobjectComfortRepo
{
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(Guid subobjectId, Guid subobjectComfortId);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task UnlinkAsync(Guid subobjectId, Guid subobjectComfortId);
}
