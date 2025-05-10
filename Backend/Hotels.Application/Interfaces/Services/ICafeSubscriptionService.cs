using Hotels.Domain.Entities.PaidServices;

namespace Hotels.Application.Interfaces.Services;

public interface ICafeSubscriptionService
{
    Task<CafeSubscription> CreateAsync(Guid cafeId, Guid paidServiceId);
}
