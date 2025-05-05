using Hotels.Domain.Common;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities;

//[Table("Nearbies")]
public class Nearby : ApplicationNamedEntity
{
    public required Guid CountrySubjectId { get; set; }
    public CountrySubject CountrySubject { get; set; } = null!;

    public Guid? ImageLinkId { get; set; }
    public NearbyImageLink? ImageLink { get; set; }

    // ===

    public virtual ICollection<Partner> Partners { get; set; } = [];

    public override string ToString()
    {
        return $"{nameof(Nearby)}_{Id}";
    }
}
