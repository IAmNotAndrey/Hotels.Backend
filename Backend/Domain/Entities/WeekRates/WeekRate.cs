using Hotels.Domain.Common;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.WeekRates;

public abstract class WeekRate : ApplicationBaseEntity, IEnumerable<decimal?>
{
    public decimal? Monday { get; set; }
    public decimal? Tuesday { get; set; }
    public decimal? Wednesday { get; set; }
    public decimal? Thursday { get; set; }
    public decimal? Friday { get; set; }
    public decimal? Saturday { get; set; }
    public decimal? Sunday { get; set; }

    [NotMapped]
    public decimal? this[DayOfWeek dayOfWeek]
    {
        get => dayOfWeek switch
        {
            DayOfWeek.Monday => Monday,
            DayOfWeek.Tuesday => Tuesday,
            DayOfWeek.Wednesday => Wednesday,
            DayOfWeek.Thursday => Thursday,
            DayOfWeek.Friday => Friday,
            DayOfWeek.Saturday => Saturday,
            DayOfWeek.Sunday => Sunday,
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
        set => _ = dayOfWeek switch
        {
            DayOfWeek.Monday => Monday = value,
            DayOfWeek.Tuesday => Tuesday = value,
            DayOfWeek.Wednesday => Wednesday = value,
            DayOfWeek.Thursday => Thursday = value,
            DayOfWeek.Friday => Friday = value,
            DayOfWeek.Saturday => Saturday = value,
            DayOfWeek.Sunday => Sunday = value,
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
    }

    public IEnumerator<decimal?> GetEnumerator()
    {
        yield return Monday;
        yield return Tuesday;
        yield return Wednesday;
        yield return Thursday;
        yield return Friday;
        yield return Saturday;
        yield return Sunday;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
