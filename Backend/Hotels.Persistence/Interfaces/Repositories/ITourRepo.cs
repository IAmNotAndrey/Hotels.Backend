using Hotels.Application.Dtos;
using Hotels.Domain.Entities;
using Hotels.Presentation.Interfaces;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITourRepo
{
    Task<IEnumerable<TourDto>> GetDtosIncludedAsync();
    Task<TourDto> GetDtoIncludedAsync(Guid id);
    Task<IEnumerable<TourDto>> GetDtosIncludedAsync(IFilterModel<Tour> filter);
}
