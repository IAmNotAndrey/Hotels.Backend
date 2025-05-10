namespace Hotels.Application.Interfaces.Services;

public interface IAdminService
{
    Task CreateAsync(string email, string password);

    /// <exception cref="InvalidOperationException">Throws when try to delete last user.</exception>
    Task DeleteAsync(string id);
    Task ConfirmModerationAsync(string userId);
}
