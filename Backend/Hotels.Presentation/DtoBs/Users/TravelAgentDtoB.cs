using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Users;

public class TravelAgentDtoB : ApplicationObjectDtoB
{
    [Url] public string? WebsiteUrl { get; set; }

    // ===

    public Guid? CountrySubjectId { get; set; }
}
