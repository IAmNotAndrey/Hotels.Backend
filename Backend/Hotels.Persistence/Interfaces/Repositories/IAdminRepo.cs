using Hotels.Application.Dtos.Users;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IAdminRepo
{
    Task<IEnumerable<PartnerDto>> GetPartnersOnModerationAsync();
    Task<IEnumerable<TravelAgentDto>> GetTravelAgentsOnModerationAsync();
}
