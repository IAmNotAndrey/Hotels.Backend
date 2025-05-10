using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Hotels.Infrastructure.Services;

public class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _appUM;

    public ApplicationUserService(UserManager<ApplicationUser> appUM)
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
