using Hotels.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Hotels.Application.Interfaces.Services;

public interface INearbyService
{
    Task SetImageLinkAsync(Guid id, IFormFile file);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(string partnerId, Guid nearbyId);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task UninkAsync(string partnerId, Guid nearbyId);
}
