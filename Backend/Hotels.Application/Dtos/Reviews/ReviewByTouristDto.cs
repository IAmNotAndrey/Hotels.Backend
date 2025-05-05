namespace Hotels.Application.Dtos.Reviews;

public abstract class ReviewByTouristDto : ReviewDto
{
    public required string TouristId { get; set; }
}
