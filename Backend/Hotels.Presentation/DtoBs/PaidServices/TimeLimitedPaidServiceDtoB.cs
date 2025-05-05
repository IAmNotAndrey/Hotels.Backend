using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.PaidServices;

public abstract class TimeLimitedPaidServiceDtoB : PaidServiceDtoB
{
    [Required] public TimeSpan Duration { get; set; }
}
