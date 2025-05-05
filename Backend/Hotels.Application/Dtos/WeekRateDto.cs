using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos;

public class WeekRateDto : ApplicationBaseEntityDto
{
    public decimal? Monday { get; set; }
    public decimal? Tuesday { get; set; }
    public decimal? Wednesday { get; set; }
    public decimal? Thursday { get; set; }
    public decimal? Friday { get; set; }
    public decimal? Saturday { get; set; }
    public decimal? Sunday { get; set; }
}
