using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.Contacts;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Users;

public abstract class ApplicationObject : ApplicationUser, IHasTitleImage<ObjectImageLink>, IAdvertising, IPublicationStatus
{
    public string? Description { get; set; }

    public string? Coordinates { get; set; }
    public string? Address { get; set; }

    public bool IsPromoSeries { get; set; } = false;

    /// <summary>
    /// Будет ли высвечиваться, как реклама
    /// </summary>
    public bool IsAdvertised { get; set; } = false;
    public PublicationStatus PublicationStatus { get; set; } = PublicationStatus.Unpublished;

    [NotMapped] public abstract bool IsPublished { get; }

    [NotMapped] public ObjectImageLink? TitleImageLink => ImageLinks.FirstOrDefault(e => e.IsTitle);

    // ===

    public ICollection<ApplicationObjectContact> Contacts { get; set; } = [];
    public ICollection<ObjectImageLink> ImageLinks { get; set; } = [];
}
