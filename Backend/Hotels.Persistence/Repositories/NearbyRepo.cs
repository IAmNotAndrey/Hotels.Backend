using AutoMapper;
using Hotels.Application.Dtos;
using Hotels.Domain.Entities;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class NearbyRepo : INearbyRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;

    public NearbyRepo(ApplicationContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NearbyDto>> GetDtosIncludedAsync()
    {
        var dtos = await _db.Nearbies
            .AsNoTracking()
            .Include(e => e.ImageLink)
            .Select(e => _mapper.Map<NearbyDto>(e))
            .ToArrayAsync();
        return dtos;
    }

    public async Task<NearbyDto> GetDtoIncludedAsync(Guid id)
    {
        Nearby nearby = await _db.Nearbies
            .AsNoTracking()
            .Include(e => e.ImageLink)
            .FirstAsync(e => e.Id == id);
        NearbyDto dto = _mapper.Map<NearbyDto>(nearby);
        return dto;
    }
}
