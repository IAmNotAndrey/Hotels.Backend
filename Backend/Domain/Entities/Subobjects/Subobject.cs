using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.Feeds;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Entities.WeekRates;
using Hotels.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Subobjects;

public abstract class Subobject : ApplicationNamedEntity, ICreatedAt, IHasTitleImage<SubobjectImageLink>, IBedroomCount
{
    public abstract string TypeName { get; init; } // fixme! плохая реализация

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public required string Description { get; set; }

    /// <summary>
    /// Площадь, м2
    /// </summary>
    public float Square { get; set; }

    public int MinDaysForOrder { get; set; }

    /// <summary>
    /// Цена за дополнительное место
    /// </summary>
    public decimal? ExtraPlacePrice { get; set; }

    /// <summary>
    /// Есть ли возможность увеличить вместимость подобъекта (например, для ребёнка)
    /// </summary>
    [NotMapped] public bool HasExtraPlaces => ExtraPlacePrice != null;

    /// <summary>
    /// Количество спальных мест у подобъекта
    /// </summary>
    public int BedCount { get; set; }

    /// <summary>
    /// Количество спален
    /// </summary>
    public int BedroomCount { get; set; }

    /// <summary>
    /// Ориентировочные цены для разных сезонов { 0: summer; 1: summer winter } (см. Season)
    /// </summary>
    public decimal?[] SeasonPrices { get; set; } = new decimal?[2];

    /// <summary>
    /// Сезон, в который подобъект доступен
    /// </summary>
    public required SubobjectSeason Season { get; set; } = SubobjectSeason.Summer;

    /// <summary>
    /// Возвращает первый попавшийся <see cref="SubobjectImageLink"/> с <see cref="TitledImageLink.IsTitle"/> == <see langword="true"/>
    /// </summary>
    [NotMapped] public SubobjectImageLink? TitleImageLink => ImageLinks.FirstOrDefault(e => e.IsTitle);

    // ===

    public required string PartnerId { get; set; }
    public Partner Partner { get; set; } = null!;

    /// <summary>
    /// Еженедельная плата за проживание.
    /// </summary>
    public SubobjectWeekRate? WeekRate { get; set; }

    public ICollection<SubobjectComfort> Comforts { get; set; } = [];
    public ICollection<SubobjectFeed> Feeds { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<SubobjectImageLink> ImageLinks { get; set; } = [];
    public ICollection<Toilet> Toilets { get; set; } = [];
    public ICollection<Bathroom> Bathrooms { get; set; } = [];


    public override string ToString()
    {
        return $"{nameof(Subobject)}_{Id}";
    }
}
