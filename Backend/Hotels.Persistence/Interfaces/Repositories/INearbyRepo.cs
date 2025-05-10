using Hotels.Application.Dtos;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface INearbyRepo
{
    Task<IEnumerable<NearbyDto>> GetDtosIncludedAsync();
    Task<NearbyDto> GetDtoIncludedAsync(Guid id);
}
