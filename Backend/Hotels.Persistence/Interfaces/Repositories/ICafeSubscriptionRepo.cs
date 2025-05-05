using Hotels.Application.Dtos.Subscriptions;
using Hotels.Domain.Entities.PaidServices;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ICafeSubscriptionRepo
{
    Task<IEnumerable<CafeSubscriptionDto>> GetDtosIncludedByCafeAsync(Guid cafeId);
    Task<CafeSubscription> CreateAsync(Guid cafeId, Guid paidServiceId);
}
