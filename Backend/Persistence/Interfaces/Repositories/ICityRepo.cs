using Hotels.Application.Dtos.Regions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ICityRepo
{
    Task<IEnumerable<CityDto>> GetDtosByCountrySubjectAsync(Guid countrySubjectId);
}
