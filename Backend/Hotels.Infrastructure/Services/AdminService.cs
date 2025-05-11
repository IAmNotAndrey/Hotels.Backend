using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Hotels.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationContext _db;
    private readonly UserManager<Admin> _userManager;
    private readonly IGenericRepo<Admin, string> _genRepo;

    public AdminService(ApplicationContext db,
                        UserManager<Admin> userManager,
                        IGenericRepo<Admin, string> genRepo)
    {
        _db = db;
        _userManager = userManager;
        _genRepo = genRepo;
    }

    public async Task ConfirmModerationAsync(string userId)
    {
        Admin admin = await _genRepo.GetByIdAsync(userId, asNoTracking: false);
        admin.AccountStatus = AccountStatus.Active;
        await _db.SaveChangesAsync();
    }

    public async Task CreateAsync(string email, string password)
    {
        Admin admin = new() { Email = email, EmailConfirmed = true };
        IdentityResult identityResult = await _userManager.CreateAsync(admin, password);
        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException($"User manager has thrown following errors when creating an Admin: {identityResult.Errors}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        Admin admin = await _userManager.FindByIdAsync(id)
            ?? throw new EntityNotFoundException($"{nameof(Admin)} wasn't found by id '{id}'.");

        if (_userManager.Users.Count() <= 1)
        {
            throw new InvalidOperationException("The operation cannot be exectued because you are trying to delete the last admin.");
        }
        await _userManager.DeleteAsync(admin);
    }
}
