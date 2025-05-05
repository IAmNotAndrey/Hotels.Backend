using Hotels.Application.Dtos;
using Hotels.ClassLibrary.Interfaces;
using Hotels.Domain.Entities;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITourRepo
{
    Task<IEnumerable<TourDto>> GetDtosIncludedAsync();
    Task<TourDto> GetDtoIncludedAsync(Guid id);
    Task<IEnumerable<TourDto>> GetDtosIncludedAsync(IFilterModel<Tour> filter);
}
