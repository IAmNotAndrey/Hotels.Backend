using AutoMapper;
using Hotels.Application.Dtos.Regions;
using Hotels.Domain.Entities.Places;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class CityRepo : ICityRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;

    public CityRepo(ApplicationContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CityDto>> GetDtosByCountrySubjectAsync(Guid countrySubjectId)
    {
        City[] cities = await _db.Cities
            .Where(e => e.CountrySubjectId == countrySubjectId)
            .ToArrayAsync();
        IEnumerable<CityDto> dtos = cities.Select(e => _mapper.Map<CityDto>(e));
        return dtos;
    }
}
