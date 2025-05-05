using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.EntityTypes;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.Reviews;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities;

public class Tour : ApplicationNamedEntity, IPublicationStatus, IHasTitleImage<TourImageLink>, IHasType<TourType>, IHasAverageRating<TourReview>
{
    public PublicationStatus PublicationStatus { get; set; } = PublicationStatus.Unpublished;

    /// <summary>
    /// Продолжительность (в днях)
    /// </summary>
    public int DaysDuration { get; set; }

    /// <summary>
    /// Минимальное количество туристов в группе
    /// </summary>
    public int MinPeople { get; set; }

    /// <summary>
    /// Максимальное количество туристов в группе
    /// </summary>
    public int MaxPeople { get; set; }

    /// <summary>
    /// Стоимость с туриста/группы (в зависимости от значения <see cref="PriceType"/>) в рублях
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Тип цены для <see cref="Price"/>
    /// </summary>
    public TourPriceType PriceType { get; set; }

    public string Description { get; set; }

    // fixme HashSet!
    /// <summary>
    /// Ближайшие даты начала тура
    /// </summary>
    public DateOnly[] UpcomingStartDates { get; set; } = [];
    public TourSeason[] Seasons { get; set; } = [];

    [NotMapped] public TourImageLink? TitleImageLink => ImageLinks.FirstOrDefault(e => e.IsTitle);
    [NotMapped] public float? AverageRating => Reviews.Count > 0 ? Reviews.Average(e => e.Rating) : null;

    // ===

    public required string TravelAgentId { get; set; }
    public TravelAgent TravelAgent { get; set; } = null!;

    /// <summary>
    /// Вид тура (водный, конный, ..)
    /// </summary>
    public Guid? TypeId { get; set; }
    public TourType? Type { get; set; } = null!;

    // note : необходима ли эта привязка? Если Tour будет привязан к TravelManager, тогда нет смысла
    public required Guid CountrySubjectId { get; set; }
    public CountrySubject CountrySubject { get; set; } = null!;

    public ICollection<TourImageLink> ImageLinks { get; set; } = [];
    public ICollection<TourReview> Reviews { get; set; } = [];

    public override string ToString()
    {
        return $"{nameof(Tour)}_{Id}";
    }
}
