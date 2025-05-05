using Hotels.Presentation.Attributes;
using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Reviews;

public abstract class ReviewDtoB : ApplicationBaseEntityDtoB
{
    [Required, StepValidation(1, 5, 0.5f)]
    public float Rating { get; set; }

    [Required, StringLength(5000, MinimumLength = 30)]
    public string Comment { get; set; } = null!;
}
