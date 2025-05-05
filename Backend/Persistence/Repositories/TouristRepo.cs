using AutoMapper;
using Hotels.Application.Dtos;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class TouristRepo : ITouristRepo
{
    private readonly IGenericRepo<Tourist, string> _repo;
    private readonly IMapper _mapper;

    public TouristRepo(IGenericRepo<Tourist, string> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TouristDto>> GetDtosIncludedAsync()
    {
        return await IncludeRelations(_repo.Entities)
               .Select(e => _mapper.Map<TouristDto>(e))
               .ToArrayAsync();
    }

    public async Task<TouristDto> GetDtoIncludedAsync(string id)
    {
        Tourist tourist = await IncludeRelations(_repo.Entities).FirstAsync(e => e.Id == id);
        TouristDto dto = _mapper.Map<TouristDto>(tourist);
        return dto;
    }

    private static IQueryable<Tourist> IncludeRelations(IQueryable<Tourist> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(t => t.Bookings)
            .Include(t => t.ObjectReviews);
    }
}
