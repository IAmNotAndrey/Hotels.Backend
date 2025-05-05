using Hotels.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.PaidServices;

public class TravelAgentSubscription : Subscription
{
    [NotMapped] public override DateTime ExpiresAt => CreatedAt + PaidService.Duration;
    [NotMapped] public override bool IsActive => DateTime.UtcNow <= ExpiresAt;

    // ===

    public required string TravelAgentId { get; set; }
    public TravelAgent TravelAgent { get; set; } = null!;

    public required Guid PaidServiceId { get; set; }
    public TravelAgentTimeLimitedPaidService PaidService { get; set; } = null!;
}
