using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Places;

public class City : Region
{
    public required Guid CountrySubjectId { get; set; }
    public CountrySubject CountrySubject { get; set; } = null!;

    public ICollection<Partner> Partners { get; set; } = [];
}
