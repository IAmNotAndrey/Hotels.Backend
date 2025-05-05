using Hotels.Application.Dtos.Comforts;
using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.Feeds;
using Hotels.Application.Dtos.StaticFiles;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos.Subobjects;

public class SubobjectDto : ApplicationNamedEntityDto
{
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; } = null!;
    public float Square { get; set; }
    public int MinDaysForOrder { get; set; }
    public decimal? ExtraPlacePrice { get; set; }
    public bool HasExtraPlaces { get; set; }
    public int BedCount { get; set; }
    public decimal?[] SeasonPrices { get; set; } = null!;
    public SubobjectSeason Season { get; set; }
    public SubobjectImageLinkDto? TitleImageLink { get; set; }


    // ===

    public string PartnerId { get; set; } = null!;
    public SubobjectWeekRateDto? WeekRate { get; set; }

    // todo заменить Dto
    public ICollection<SubobjectComfortDto> Comforts { get; set; } = [];
    public ICollection<SubobjectFeedDto> Feeds { get; set; } = [];
    public ICollection<BookingDto> Bookings { get; set; } = [];
    public ICollection<SubobjectImageLinkDto> ImageLinks { get; set; } = [];
    public ICollection<ToiletDto> Toilets { get; set; } = [];
    public ICollection<BathroomDto> Bathrooms { get; set; } = [];
}
