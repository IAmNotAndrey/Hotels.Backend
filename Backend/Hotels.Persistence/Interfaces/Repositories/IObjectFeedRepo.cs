using Hotels.Application.Exceptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IObjectFeedRepo
{
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(string partnerId, Guid feedId);

    /// <exception cref="EntityNotFoundException"></exception>
	/// <exception cref="InvalidOperationException"></exception>
    Task UnlinkAsync(string partnerId, Guid feedId);
}
