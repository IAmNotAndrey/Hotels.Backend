using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public class SubobjectWeekRateDtoB : WeekRateDtoB
{
    [Required] public Guid SubobjectId { get; set; }
}
