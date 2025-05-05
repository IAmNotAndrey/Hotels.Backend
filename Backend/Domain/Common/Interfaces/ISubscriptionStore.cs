using Hotels.Domain.Entities.PaidServices;

namespace Hotels.Domain.Common.Interfaces;

public interface ISubscriptionStore<TSubscription> where TSubscription : Subscription
{
    ICollection<TSubscription> Subscriptions { get; }
}
