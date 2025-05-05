using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Reviews;

public class PartnerReviewDtoB : ReviewByTouristDtoB
{
    [Required] public required string PartnerId { get; init; }
}
