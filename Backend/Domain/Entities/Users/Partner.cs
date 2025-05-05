using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.EntityTypes;
using Hotels.Domain.Entities.Feeds;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.Reviews;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Users;

/// <summary>
/// Партнёр, он же "Объект"
/// </summary>
public class Partner : ApplicationObject, IHasType<ObjectType>, IHasAverageRating<PartnerReview>
{
    [NotMapped] public override IdentityRole Role => new(nameof(Partner));

    [NotMapped] public SubobjectSeason Season => GetObjectSeason();
    [NotMapped] public decimal? MinimalWeekRate => GetMinimalWeekRate();
    [NotMapped] public override bool IsPublished => PublicationStatus == PublicationStatus.Published && AccountStatus == AccountStatus.Active;
    [NotMapped] public float? AverageRating => Reviews.Count > 0 ? Reviews.Average(e => e.Rating) : null;

    // ===

    // NOTE: Связи 1:M сделаны Nullable, чтобы не работало каскадное удаление (пример: удаление "ObjectType" не должно приводить к удалению всех связанных "Partners")

    public Guid? TypeId { get; set; }
    public virtual ObjectType? Type { get; set; } = null!;

    /// <summary>
    /// Населённый пункт
    /// </summary>
    public Guid? CityId { get; set; }
    public City? City { get; set; } = null!;

    public ICollection<ObjectComfort> Comforts { get; set; } = [];
    public ICollection<ObjectFeed> Feeds { get; set; } = [];
    public ICollection<Nearby> Nearbies { get; set; } = [];
    public ICollection<Subobject> Subobjects { get; set; } = [];
    public ICollection<PartnerReview> Reviews { get; set; } = [];

    /// <summary>
    /// Получает сезон для объекта, исходя из его подобъектов
    /// </summary>
    private SubobjectSeason GetObjectSeason()
    {
        if (Subobjects.Any(s => s.Season == SubobjectSeason.SummerAndWinter))
        {
            return SubobjectSeason.SummerAndWinter;
        }
        return SubobjectSeason.Summer;
    }

    /// <summary>
    /// Получает минимальную стоимость для объекта путём нахождения самой минимальной стоимости проживания для всех подобъектов
    /// </summary>
    /// <returns>null, если не удалось найти минимальную цену: нет подобъектов, у подобъектов все дни помечены как недоступные для брони</returns>
    private decimal? GetMinimalWeekRate()
    {
        decimal minRate = decimal.MaxValue;
        foreach (var so in Subobjects)
        {
            // Находим минимальную цену среди всех `WeekRates`, которые не равны null
            decimal? subobjectMinRate = so.WeekRate?.Where(rate => rate.HasValue).Min();

            // Если у текущего подобъекта есть меньшая ставка, обновляем общую минимальную ставку
            if (subobjectMinRate.HasValue)
            {
                if (subobjectMinRate.Value < minRate)
                {
                    minRate = subobjectMinRate.Value;
                }
            }
        }
        return minRate != decimal.MaxValue ? minRate : null;
    }

    public override string ToString()
    {
        return $"{nameof(Partner)}_{Id}";
    }
}
