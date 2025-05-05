using Hotels.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Hotels.Infrastructure.Factories;

public class CustomUserClaimsPrincipalFactory<TUser> : UserClaimsPrincipalFactory<TUser> where TUser : ApplicationUser
{
    private readonly ILogger<CustomUserClaimsPrincipalFactory<TUser>> _logger;

    public CustomUserClaimsPrincipalFactory(
        UserManager<TUser> userManager,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<CustomUserClaimsPrincipalFactory<TUser>> logger)
        : base(userManager, optionsAccessor)
    {
        _logger = logger;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
        ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

        // Добавляем кастомный Claim на основе свойства Role
        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name!));
        _logger.LogInformation("A Role Claim was added with value '{Name}' to '{User}'", user.Role.Name, user);

        return identity;
    }
}
