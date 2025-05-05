using Hotels.Application.Exceptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IObjectComfortRepo
{
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(string partnerId, Guid objectComfortId);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task UnlinkAsync(string partnerId, Guid objectComfortId);
}
