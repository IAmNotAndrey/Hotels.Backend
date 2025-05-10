using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Persistence.Interfaces.Repositories;

namespace Hotels.Infrastructure.Services;

public class CafeSubscriptionService : ICafeSubscriptionService
{
    private readonly IGenericRepo<Cafe, Guid> _cafeRepo;
    private readonly IGenericRepo<CafeTimeLimitedPaidService, Guid> _cafeTLPSRepo;
    private readonly IGenericRepo<CafeSubscription, Guid> _repo;

    public CafeSubscriptionService(IGenericRepo<Cafe, Guid> cafeRepo,
                                   IGenericRepo<CafeTimeLimitedPaidService, Guid> cafeTLPSRepo,
                                   IGenericRepo<CafeSubscription, Guid> repo)
    {
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
}
