using Hotels.Application.Dtos.Subscriptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ICafeSubscriptionRepo
{
    Task<IEnumerable<CafeSubscriptionDto>> GetDtosIncludedByCafeAsync(Guid cafeId);
}
