namespace Hotels.Application.Dtos.Reviews;

public class PartnerReviewDto : ReviewByTouristDto
{
    public required string PartnerId { get; init; }
}
