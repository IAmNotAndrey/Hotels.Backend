using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Reviews;

/// <summary>
/// Отзыв, который оставляет турист
/// </summary>
public abstract class ReviewByTourist : Review
{
    public required string TouristId { get; set; }
    /// <summary>
    /// Пользователь, оставивший отзыв
    /// </summary>
    public Tourist Tourist { get; set; } = null!;
}
