using Hotels.Application.Dtos.Subscriptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ITravelAgentSubscriptionRepo
{
    Task<IEnumerable<TravelAgentSubscriptionDto>> GetDtosIncludedByTravelAgent(string travelAgentId);
}
