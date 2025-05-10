using Hotels.Application.Exceptions;

namespace Hotels.Application.Interfaces.Services;

public interface IBathroomService
{
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(Guid subobjectId, Guid bathroomId);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task UnlinkAsync(Guid subobjectId, Guid bathroomId);
}
