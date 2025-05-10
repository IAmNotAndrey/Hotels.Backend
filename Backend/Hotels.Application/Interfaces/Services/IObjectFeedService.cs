using Hotels.Application.Exceptions;

namespace Hotels.Application.Interfaces.Services;

public interface IObjectFeedService
{
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(string partnerId, Guid feedId);

    /// <exception cref="EntityNotFoundException"></exception>
	/// <exception cref="InvalidOperationException"></exception>
    Task UnlinkAsync(string partnerId, Guid feedId);
}
