using Hotels.Domain.Enums;
using Hotels.Presentation.Attributes;
using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public class TourDtoB : ApplicationBaseEntityDtoB
{
    [Length(3, 256)]
    public override required string Name { get; set; }

    [Required, Range(1, 30)]
    public int DaysDuration { get; set; }

    [Required, Range(1, 100)]
    public int MinPeople { get; set; }

    [Required, Range(1, 100), GreaterOrEqualThan(nameof(MinPeople))]
    public int MaxPeople { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required] public TourPriceType PriceType { get; set; }

    [Required, Length(25, 5000)]
    public required string Description { get; set; }

    [Required] public DateOnly[] UpcomingStartDates { get; set; } = []; // fixme HashSet
    [Required] public TourSeason[] Seasons { get; set; } = [];

    // ===

    [Required] public required string TravelAgentId { get; set; }
    public Guid? TypeId { get; set; }
    [Required] public required Guid CountrySubjectId { get; set; }
}
