using Hotels.Application.Dtos.Users;
using Hotels.Domain.Entities.Users;
using Hotels.Presentation.DtoBs.Users;
using Hotels.Presentation.Interfaces;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITravelAgentRepo
{
    Task<TravelAgentDto> GetDtoIncludedAsync(string id);
    Task<IEnumerable<TravelAgentDto>> GetDtosIncludedAsync(IFilterModel<TravelAgent> filter);
    Task<IEnumerable<TravelAgentDto>> GetDtosIncludedAsync();
    Task UpdateAsync(string id, TravelAgentDtoB dtoB);
}
