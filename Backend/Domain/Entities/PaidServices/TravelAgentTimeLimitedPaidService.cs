namespace Hotels.Domain.Entities.PaidServices;

public class TravelAgentTimeLimitedPaidService : TimeLimitedPaidService
{
    public ICollection<TravelAgentSubscription> TravelAgentSubscriptions { get; set; } = [];
}
