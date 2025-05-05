namespace Hotels.Domain.Entities.Reviews;

public class TourReview : ReviewByTourist
{
    public required Guid TourId { get; init; }
    public Tour Tour { get; init; } = null!;
}
