using Hotels.Application.Dtos.Regions;
using Hotels.Application.Exceptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ICountrySubjectRepo
{
    /// <exception cref="EntityNotFoundException"></exception>
    Task<CountrySubjectDto> GetByCityIdAsync(Guid cityId);
}
