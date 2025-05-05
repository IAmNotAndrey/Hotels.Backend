using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos.Subscriptions;

public abstract class SubscriptionDto : ApplicationBaseEntityDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}
