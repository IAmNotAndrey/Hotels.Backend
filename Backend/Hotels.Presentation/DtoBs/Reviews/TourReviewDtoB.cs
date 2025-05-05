using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Reviews;

public class TourReviewDtoB : ReviewByTouristDtoB
{
    [Required] public required Guid TourId { get; init; }
}
