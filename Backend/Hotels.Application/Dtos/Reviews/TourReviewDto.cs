namespace Hotels.Application.Dtos.Reviews;

public class TourReviewDto : ReviewByTouristDto
{
    public required Guid TourId { get; init; }
}
