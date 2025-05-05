using Hotels.Application.Dtos.Users;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IAdminRepo
{
    Task CreateAsync(string email, string password);

    /// <exception cref="InvalidOperationException">Throws when try to delete last user.</exception>
    Task DeleteAsync(string id);

    Task<IEnumerable<PartnerDto>> GetPartnersOnModerationAsync();
    Task<IEnumerable<TravelAgentDto>> GetTravelAgentsOnModerationAsync();
    Task ConfirmModerationAsync(string userId);
}
