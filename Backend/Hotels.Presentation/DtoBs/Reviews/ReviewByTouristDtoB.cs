using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Reviews;

public abstract class ReviewByTouristDtoB : ReviewDtoB
{
    [Required] public required string TouristId { get; set; }
}
