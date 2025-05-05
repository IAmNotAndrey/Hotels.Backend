namespace Hotels.Domain.Entities.PaidServices;

public class CafeTimeLimitedPaidService : TimeLimitedPaidService
{
    public ICollection<CafeSubscription> CafeSubscriptions { get; set; } = [];
}
