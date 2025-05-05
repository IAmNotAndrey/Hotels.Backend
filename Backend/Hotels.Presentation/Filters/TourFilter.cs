using Hotels.Domain.Entities;
using Hotels.Domain.Enums;
using Hotels.Presentation.Interfaces;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Hotels.Presentation.Filters;

public class TourFilter : IFilterModel<Tour>
{
    // Длительность от/до
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }

    // Туристов в группе от/до
    public int? MinPeople { get; set; }
    public int? MaxPeople { get; set; }

    public HashSet<TourSeason> Seasons { get; set; } = [];
    public HashSet<Guid> TypeIds { get; set; } = [];

    // Цена от/до
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // Ближайшие даты начала с/по
    public DateOnly? StartDateMin { get; set; }
    public DateOnly? StartDateMax { get; set; }

    public float? MinAverageRating { get; set; }

    [JsonIgnore]
    public Expression<Func<Tour, bool>> FilterExpression => t =>
        (!MinDuration.HasValue || t.DaysDuration >= MinDuration) &&
        (!MaxDuration.HasValue || t.DaysDuration <= MaxDuration) &&

        (!MinPeople.HasValue || t.MinPeople >= MinPeople) &&
        (!MaxPeople.HasValue || t.MaxPeople <= MaxPeople) &&
        // Если есть пересечение хотя бы по одному Season
        (Seasons.Count == 0 || Seasons.Any(s => t.Seasons.Any(ts => ts == s))) &&
        (TypeIds.Count == 0 || TypeIds.Any(tid => tid == t.TypeId)) &&
        // fixme? цена у Tour может быть указана как для одного человека, так и для группы. Что в фильтре?
        (!MinPrice.HasValue || t.Price >= MinPrice) &&
        (!MaxPrice.HasValue || t.Price <= MaxPrice) &&

        (!StartDateMin.HasValue || t.UpcomingStartDates.Any(usd => usd >= StartDateMin)) &&
        (!StartDateMax.HasValue || t.UpcomingStartDates.Any(usd => usd <= StartDateMax)) &&
        // note нельзя использовать `Tour.AverageRating`, т.к. он не сохраняется в БД
        (!MinAverageRating.HasValue || t.Reviews.Count > 0 && t.Reviews.Average(r => r.Rating) >= MinAverageRating);

    public void Reset()
    {
        MinDuration = null;
        MaxDuration = null;
        MinPeople = null;
        MaxPeople = null;
        Seasons = [];
        TypeIds = [];
        MinPrice = null;
        MaxPrice = null;
        StartDateMin = null;
        StartDateMax = null;
        MinAverageRating = null;
    }
}
