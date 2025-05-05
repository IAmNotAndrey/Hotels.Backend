using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Users;

public class TravelAgent : ApplicationObject, ISubscriptionStore<TravelAgentSubscription>
{
    [NotMapped] public override IdentityRole Role => new(nameof(TravelAgent));

    public string? WebsiteUrl { get; set; }

    [NotMapped] public override bool IsPublished => PublicationStatus == PublicationStatus.Published && AccountStatus == AccountStatus.Active;

    // ===

    public string? LogoLinkId { get; set; }
    public TravelAgentLogoLink? LogoLink { get; set; }

    /// <summary>
    /// По выбранному значению будет осуществляться поиск организации и публикация платных услуг.
    /// </summary>
    public Guid? CountrySubjectId { get; set; }
    public CountrySubject? CountrySubject { get; set; } = null!;


    public ICollection<TravelAgentSubscription> Subscriptions { get; set; } = [];
    public ICollection<Tour> Tours { get; set; } = [];

    public override string ToString()
    {
        return $"{nameof(TravelAgent)}_{Id}";
    }
}
