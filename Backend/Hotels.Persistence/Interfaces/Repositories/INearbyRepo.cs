using Hotels.Application.Dtos;
using Hotels.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface INearbyRepo
{
    Task<IEnumerable<NearbyDto>> GetDtosIncludedAsync();
    Task<NearbyDto> GetDtoIncludedAsync(Guid id);
    Task SetImageLinkAsync(Guid id, IFormFile file);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task LinkAsync(string partnerId, Guid nearbyId);

    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task UninkAsync(string partnerId, Guid nearbyId);
}
