using Hotels.Application.Dtos;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITouristRepo
{
    Task<TouristDto> GetDtoIncludedAsync(string id);
    Task<IEnumerable<TouristDto>> GetDtosIncludedAsync();
}
