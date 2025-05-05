using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.PaidServices;

public class CafeSubscription : Subscription
{
    [NotMapped] public override DateTime ExpiresAt => CreatedAt + PaidService.Duration;
    [NotMapped] public override bool IsActive => DateTime.UtcNow <= ExpiresAt;

    // ===

    public required Guid CafeId { get; set; }
    public Cafe Cafe { get; set; } = null!;

    public required Guid PaidServiceId { get; set; }
    public CafeTimeLimitedPaidService PaidService { get; set; } = null!;
}
