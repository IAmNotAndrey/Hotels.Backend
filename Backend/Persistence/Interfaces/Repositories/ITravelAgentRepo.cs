using Hotels.Application.Dtos.Users;
using Hotels.ClassLibrary.Business.DtoBs.Users;
using Hotels.ClassLibrary.Interfaces;
using Hotels.Domain.Entities.Users;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITravelAgentRepo
{
    Task<TravelAgentDto> GetDtoIncludedAsync(string id);
    Task<IEnumerable<TravelAgentDto>> GetDtosIncludedAsync(IFilterModel<TravelAgent> filter);
    Task<IEnumerable<TravelAgentDto>> GetDtosIncludedAsync();
    Task UpdateAsync(string id, TravelAgentDtoB dtoB);
}
