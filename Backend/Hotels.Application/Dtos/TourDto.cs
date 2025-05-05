using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.Reviews;
using Hotels.Application.Dtos.StaticFiles;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos;

public class TourDto : ApplicationNamedEntityDto
{
    public PublicationStatus PublicationStatus { get; set; }
    public int DaysDuration { get; set; }
    public int MinPeople { get; set; }
    public int MaxPeople { get; set; }
    public decimal Price { get; set; }
    public TourPriceType PriceType { get; set; }
    public required string Description { get; set; }
    // fixme HashSet
    public DateOnly[] UpcomingStartDates { get; set; } = [];
    public TourSeason[] Seasons { get; set; } = [];
    public TourImageLinkDto? TitleImageLink { get; set; }
    public float? AverageRating { get; set; }

    // ===

    public required string TravelAgentId { get; set; }
    public Guid? TypeId { get; set; }
    public required Guid CountrySubjectId { get; set; }

    public ICollection<TourImageLinkDto> ImageLinks { get; set; } = [];
    public ICollection<TourReviewDto> Reviews { get; set; } = [];
}
