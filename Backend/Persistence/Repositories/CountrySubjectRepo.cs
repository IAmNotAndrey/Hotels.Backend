using Hotels.Application.Dtos.Regions;
using Hotels.Domain.Entities.Places;
using Hotels.Persistence.Interfaces.Repositories;

namespace Hotels.Persistence.Repositories;

public class CountrySubjectRepo : ICountrySubjectRepo
{
    private readonly IGenericRepo<CountrySubject, Guid> _repo;
    private readonly IGenericRepo<City, Guid> _cityRepo;

    public CountrySubjectRepo(IGenericRepo<CountrySubject, Guid> repo, IGenericRepo<City, Guid> cityRepo)
    {
        _repo = repo;
        _cityRepo = cityRepo;
    }

    public async Task<CountrySubjectDto> GetByCityIdAsync(Guid cityId)
    {
        City city = await _cityRepo.GetByIdAsync(cityId);
        return await _repo.GetDtoAsync<CountrySubjectDto>(city.CountrySubjectId);
    }
}
