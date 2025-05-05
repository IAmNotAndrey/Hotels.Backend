namespace Hotels.Application.Dtos.Subscriptions;

public class CafeSubscriptionDto : SubscriptionDto
{
    public required Guid CafeId { get; set; }
    public CafeDto Cafe { get; set; } = null!;
}
