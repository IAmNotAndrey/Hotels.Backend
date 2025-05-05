using AutoMapper;
using Hotels.Application.Dtos;
using Hotels.Domain.Entities;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class AttractionRepo : IAttractionRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;

    public AttractionRepo(ApplicationContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AttractionDto>> GetDtosIncludedAsync()
    {
        Attraction[] incAttractions = await GetIncluded();
        AttractionDto[] dtos = incAttractions.Select(e => _mapper.Map<AttractionDto>(e)).ToArray();
        return dtos;
    }

    public async Task<AttractionDto> GetDtoIncludedAsync(Guid id)
    {
        Attraction attraction = await _db.Attractions
            .AsNoTracking()
            .Include(e => e.ImageLinks)
            .FirstAsync(e => e.Id == id);
        AttractionDto dto = _mapper.Map<AttractionDto>(attraction);
        return dto;
    }

    private async Task<Attraction[]> GetIncluded()
    {
        return await _db.Attractions
            .AsNoTracking()
            .Include(e => e.ImageLinks)
            .ToArrayAsync();
    }

    /*
	//public async Task<bool> ExistsAsync(Guid id)
	//{
	//	var service = await _db.Attractions.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
	//	return service != null;
	//}

	//public async Task<Attraction> CreateAsync(AttractionDtoB dto)
	//{
	//	CountrySubject countrySubject = await _db.CountrySubjects.FirstOrDefaultAsync(e => e.Id == dto.CountrySubjectId)
	//		?? throw new EntityNotFoundException($"{nameof(CountrySubject)} wasn't found by id '{dto.CountrySubjectId}'");

	//	Attraction attraction = _mapper.Map<Attraction>(dto);
	//	await _db.Attractions.AddAsync(attraction);
	//	await _db.SaveChangesAsync();

	//	return attraction;
	//}

	//public async Task UpdateAsync(Guid id, AttractionDtoB dto)
	//{
	//	Attraction attraction = await _db.Attractions
	//		.FirstOrDefaultAsync(e => e.Id == id)
	//		?? throw new ArgumentException($"{nameof(Attraction)} wasn't found by id '{id}'", nameof(id));
	//	_mapper.Map(dto, attraction);
	//	await _db.SaveChangesAsync();
	//}

	//public async Task DeleteAsync(Guid id)
	//{
	//	var attraction = await _db.Attractions.FirstOrDefaultAsync(e => e.Id == id)
	//		?? throw new ArgumentException($"{nameof(Attraction)} wasn't found by id '{id}'", nameof(id));
	//	_db.Attractions.Remove(attraction);
	//	await _db.SaveChangesAsync();
	//}
	*/
}
