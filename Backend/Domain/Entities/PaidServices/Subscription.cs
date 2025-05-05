using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;

namespace Hotels.Domain.Entities.PaidServices;

public abstract class Subscription : ApplicationBaseEntity, ICreatedAt
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public abstract DateTime ExpiresAt { get; }
    public abstract bool IsActive { get; }
}
