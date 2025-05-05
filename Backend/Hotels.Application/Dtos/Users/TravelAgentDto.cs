using Hotels.Application.Dtos.StaticFiles;

namespace Hotels.Application.Dtos.Users;

public class TravelAgentDto : ApplicationObjectDto
{
    public string? WebsiteUrl { get; set; }

    // ===

    public TravelAgentLogoLinkDto? LogoLink { get; set; }
    public Guid? CountrySubjectId { get; set; }
    public ICollection<TourDto> Tours { get; set; } = [];
}
