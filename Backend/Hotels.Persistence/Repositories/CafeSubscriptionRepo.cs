using AutoMapper;
using Hotels.Application.Dtos.Subscriptions;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class CafeSubscriptionRepo : ICafeSubscriptionRepo
{
    private readonly IMapper _mapper;
    private readonly IGenericRepo<CafeSubscription, Guid> _repo;

    public CafeSubscriptionRepo(IMapper mapper,
                                IGenericRepo<CafeSubscription, Guid> repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<IEnumerable<CafeSubscriptionDto>> GetDtosIncludedByCafeAsync(Guid cafeId)
    {
        CafeSubscriptionDto[] dtos = await IncludeRelations(_repo.Entities)
           .Select(e => _mapper.Map<CafeSubscriptionDto>(e))
           .ToArrayAsync();
        return dtos;
    }

    private static IQueryable<CafeSubscription> IncludeRelations(IQueryable<CafeSubscription> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.PaidService);
    }
}
