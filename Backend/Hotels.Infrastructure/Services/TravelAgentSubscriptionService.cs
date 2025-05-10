using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Persistence.Interfaces.Repositories;

namespace Hotels.Infrastructure.Services;

public class TravelAgentSubscriptionService : ITravelAgentSubscriptionService
{
    private readonly IGenericRepo<TravelAgentSubscription, Guid> _repo;

    public TravelAgentSubscriptionService(IGenericRepo<TravelAgentSubscription, Guid> repo)
    {
        _repo = repo;
    }

    public async Task<TravelAgentSubscription> CreateAsync(string travelAgentId, Guid paidServiceId)
    {
        TravelAgentSubscription sub = new() { TravelAgentId = travelAgentId, PaidServiceId = paidServiceId };
        return await _repo.AddAsync(sub);
    }
}
