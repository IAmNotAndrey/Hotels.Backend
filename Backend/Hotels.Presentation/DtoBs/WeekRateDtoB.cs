using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public abstract class WeekRateDtoB
{
    [Range(0, double.MaxValue)] public decimal? Monday { get; set; }
    [Range(0, double.MaxValue)] public decimal? Tuesday { get; set; }
    [Range(0, double.MaxValue)] public decimal? Wednesday { get; set; }
    [Range(0, double.MaxValue)] public decimal? Thursday { get; set; }
    [Range(0, double.MaxValue)] public decimal? Friday { get; set; }
    [Range(0, double.MaxValue)] public decimal? Saturday { get; set; }
    [Range(0, double.MaxValue)] public decimal? Sunday { get; set; }
}
