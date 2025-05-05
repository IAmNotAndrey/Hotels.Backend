using Hotels.Domain.Enums;
using Hotels.Presentation.Attributes;
using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public class CafeDtoB : ApplicationBaseEntityDtoB
{
    [Required] public string Description { get; set; } = null!;
    [Url] public string? WebsiteUrl { get; set; }
    [Required] public SubobjectSeason Season { get; set; }
    [Required] public string Address { get; set; } = null!;
    [Coordinates] public string? Coordinates { get; set; } = null!;
    public decimal AverageCheck { get; set; }

    // Work time
    public TimeOnly? WeekDayFrom { get; set; }
    public TimeOnly? WeekDayTo { get; set; }
    public TimeOnly? WeekendFrom { get; set; }
    public TimeOnly? WeekendTo { get; set; }
    [Required] public bool ShowWorkingHours { get; set; }

    // TODO : Menu

    // ===

    [Required] public Guid CountrySubjectId { get; set; }
}
