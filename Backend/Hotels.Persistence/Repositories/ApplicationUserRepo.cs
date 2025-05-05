using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Hotels.Persistence.Repositories;

public class ApplicationUserRepo : IApplicationUserRepo
{
    private readonly UserManager<ApplicationUser> _appUM;

    public ApplicationUserRepo(UserManager<ApplicationUser> appUM)
    {
        _appUM = appUM;
    }

    public async Task ChangeNameAsync(string id, string name)
    {
        ApplicationUser user = await _appUM.FindByIdAsync(id)
            ?? throw new EntityNotFoundException($"{nameof(ApplicationUser)} wasn't found.");
        user.Name = name;
        await _appUM.UpdateAsync(user);
    }

    public async Task<bool> IsUserAllowedAsync(ClaimsPrincipal user, string idToCompare)
    {
        ApplicationUser? requester = await _appUM.GetUserAsync(user);
        if (requester == null)
        {
            return false;
        }
        if (requester is Admin)
        {
            return true;
        }
        if (requester.Id != idToCompare)
        {
            return false;
        }
        return true;
    }
}
