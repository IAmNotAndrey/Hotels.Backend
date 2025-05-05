using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.Contacts;
using Hotels.Application.Dtos.StaticFiles;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos;

public class CafeDto : ApplicationNamedEntityDto
{
    public string Description { get; set; } = null!;
    public string? WebsiteUrl { get; set; }
    public SubobjectSeason Season { get; set; }
    public string Address { get; set; } = null!;
    public string? Coordinates { get; set; } = null!;
    public decimal AverageCheck { get; set; }

    // Work time
    public TimeOnly? WeekDayFrom { get; set; }
    public TimeOnly? WeekDayTo { get; set; }
    public TimeOnly? WeekendFrom { get; set; }
    public TimeOnly? WeekendTo { get; set; }
    public bool ShowWorkingHours { get; set; }

    public CafeMenuFileLinkDto Menu { get; set; } = null!;

    public CafeImageLinkDto? TitleImageLink { get; set; }

    // ===

    public Guid CountrySubjectId { get; set; }

    public ICollection<CafeContactDto> Contacts { get; set; } = [];
    public ICollection<CafeImageLinkDto> ImageLinks { get; set; } = [];
}
