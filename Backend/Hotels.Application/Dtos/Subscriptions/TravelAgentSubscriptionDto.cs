namespace Hotels.Application.Dtos.Subscriptions;

public class TravelAgentSubscriptionDto : SubscriptionDto
{
    public string TravelAgentId { get; set; } = null!;
    public Guid PaidServiceId { get; set; }
}
