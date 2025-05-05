using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Reviews;

public class PartnerReview : ReviewByTourist
{
    public required string PartnerId { get; init; }
    /// <summary>
    /// На кого оставляют отзыв
    /// </summary>
    public Partner Partner { get; init; } = null!;
}
