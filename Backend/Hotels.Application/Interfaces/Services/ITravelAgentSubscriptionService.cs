using Hotels.Domain.Entities.PaidServices;

namespace Hotels.Application.Interfaces.Services;

public interface ITravelAgentSubscriptionService
{
    Task<TravelAgentSubscription> CreateAsync(string travelAgentId, Guid paidServiceId);
}
