using Hotels.Application.Dtos.Subscriptions;
using Hotels.Domain.Entities.PaidServices;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITravelAgentSubscriptionRepo
{
    Task<IEnumerable<TravelAgentSubscriptionDto>> GetDtosIncludedByTravelAgent(string travelAgentId);
    Task<TravelAgentSubscription> CreateAsync(string travelAgentId, Guid paidServiceId);
}
