using Hotels.Application.Exceptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IBathroomRepo
{
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(Guid subobjectId, Guid bathroomId);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task UnlinkAsync(Guid subobjectId, Guid bathroomId);
}
