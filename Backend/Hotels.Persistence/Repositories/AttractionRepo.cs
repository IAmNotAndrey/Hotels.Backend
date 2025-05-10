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
}
