using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.Contacts;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities;

public class Cafe : ApplicationNamedEntity, IHasTitleImage<CafeImageLink>, ISubscriptionStore<CafeSubscription>
{
    public string Description { get; set; } = null!;
    public string? WebsiteUrl { get; set; }
    public SubobjectSeason Season { get; set; }
    public string Address { get; set; } = null!;
    public string? Coordinates { get; set; }
    public decimal AverageCheck { get; set; }

    // Work time
    public TimeOnly? WeekDayFrom { get; set; }
    public TimeOnly? WeekDayTo { get; set; }
    public TimeOnly? WeekendFrom { get; set; }
    public TimeOnly? WeekendTo { get; set; }
    public bool ShowWorkingHours { get; set; }

    // TODO : Menu

    [NotMapped] public CafeImageLink? TitleImageLink => ImageLinks.FirstOrDefault(e => e.IsTitle);

    // ===

    public Guid CountrySubjectId { get; set; }
    public CountrySubject CountrySubject { get; set; } = null!;

    public Guid MenuId { get; set; }
    public CafeMenuFileLink? Menu { get; set; } = null!;

    public ICollection<CafeContact> Contacts { get; set; } = [];


    public ICollection<CafeImageLink> ImageLinks { get; set; } = [];
    public ICollection<CafeSubscription> Subscriptions { get; set; } = [];


    public override string ToString()
    {
        return $"{nameof(Cafe)}_{Id}";
    }
}
