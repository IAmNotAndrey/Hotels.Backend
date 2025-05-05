using AutoMapper;
using Hotels.Application.Dtos;
using Hotels.ClassLibrary.Interfaces;
using Hotels.Domain.Entities;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class TourRepo : ITourRepo
{
    private readonly IGenericRepo<Tour, Guid> _repo;
    private readonly IMapper _mapper;

    public TourRepo(IGenericRepo<Tour, Guid> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TourDto>> GetDtosIncludedAsync()
    {
        return await IncludeRelations(_repo.Entities)
            .Select(e => _mapper.Map<TourDto>(e))
            .ToArrayAsync();
    }

    public async Task<TourDto> GetDtoIncludedAsync(Guid id)
    {
        Tour tour = await IncludeRelations(_repo.Entities).FirstAsync(e => e.Id == id);
        TourDto dto = _mapper.Map<TourDto>(tour);
        return dto;
    }

    public async Task<IEnumerable<TourDto>> GetDtosIncludedAsync(IFilterModel<Tour> filter)
    {
        return await IncludeRelations(_repo.Entities)
            .Where(filter.FilterExpression)
            .Select(e => _mapper.Map<TourDto>(e))
            .ToArrayAsync();
    }

    private static IQueryable<Tour> IncludeRelations(IQueryable<Tour> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.ImageLinks)
            .Include(e => e.Reviews);
    }
}
