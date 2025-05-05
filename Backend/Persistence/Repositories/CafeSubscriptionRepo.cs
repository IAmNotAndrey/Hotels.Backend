using AutoMapper;
using Hotels.Application.Dtos.Subscriptions;
using Hotels.Application.Exceptions;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class CafeSubscriptionRepo : ICafeSubscriptionRepo
{
    private readonly IMapper _mapper;
    private readonly IGenericRepo<Cafe, Guid> _cafeRepo;
    private readonly IGenericRepo<CafeTimeLimitedPaidService, Guid> _cafeTLPSRepo;
    private readonly IGenericRepo<CafeSubscription, Guid> _repo;

    public CafeSubscriptionRepo(IMapper mapper,
                                IGenericRepo<Cafe, Guid> cafeRepo,
                                IGenericRepo<CafeTimeLimitedPaidService, Guid> cafeTLPSRepo,
                                IGenericRepo<CafeSubscription, Guid> repo)
    {
        _mapper = mapper;
        _cafeRepo = cafeRepo;
        _cafeTLPSRepo = cafeTLPSRepo;
        _repo = repo;
    }

    public async Task<CafeSubscription> CreateAsync(Guid cafeId, Guid paidServiceId)
    {
        if (!await _cafeRepo.ExistsAsync(cafeId))
        {
            throw new EntityNotFoundException($"{nameof(Cafe)} wasn't found.");
        }
        if (!await _cafeTLPSRepo.ExistsAsync(paidServiceId))
        {
            throw new EntityNotFoundException($"{nameof(CafeTimeLimitedPaidService)} wasn't found.");
        }
        CafeSubscription sub = new() { CafeId = cafeId, PaidServiceId = paidServiceId };
        return await _repo.AddAsync(sub);
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
