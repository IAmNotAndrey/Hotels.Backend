using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.StaticFiles;

public class TravelAgentLogoLink : StaticFile
{
    public required string TravelAgentId { get; set; }
    public TravelAgent TravelAgent { get; set; } = null!;
}
