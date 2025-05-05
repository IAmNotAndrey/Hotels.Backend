using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities;

public class Attraction : ApplicationNamedEntity, IHasTitleImage<AttractionImageLink>, IPublicationStatus
{
    public string Description { get; set; } = null!;
    public string? Coordinates { get; set; }
    public PublicationStatus PublicationStatus { get; set; } = PublicationStatus.Published;

    [NotMapped] public AttractionImageLink? TitleImageLink => ImageLinks.FirstOrDefault(e => e.IsTitle);

    // ===

    public Guid CountrySubjectId { get; set; }
    public CountrySubject CountrySubject { get; set; } = null!;

    public ICollection<AttractionImageLink> ImageLinks { get; set; } = [];

    public override string ToString()
    {
        return $"{nameof(Attraction)}_{Id}";
    }
}
