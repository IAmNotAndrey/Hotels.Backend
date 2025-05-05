using AutoMapper;
using Hotels.Application.Dtos.Users;
using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class AdminRepo : IAdminRepo
{
    private readonly UserManager<Admin> _userManager;
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;
    private readonly IGenericRepo<Admin, string> _genRepo;

    public AdminRepo(UserManager<Admin> userManager,
                     ApplicationContext db,
                     IMapper mapper,
                     IGenericRepo<Admin, string> genRepo)
    {
        _userManager = userManager;
        _db = db;
        _mapper = mapper;
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
        await _userManager.CreateAsync(admin, password);
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

    public async Task<IEnumerable<PartnerDto>> GetPartnersOnModerationAsync()
    {
        return await _db.Partners
            .Where(e => e.AccountStatus == AccountStatus.OnModeration)
            .Select(e => _mapper.Map<PartnerDto>(e))
            .ToArrayAsync();
    }

    public async Task<IEnumerable<TravelAgentDto>> GetTravelAgentsOnModerationAsync()
    {
        return await _db.TravelAgents
            .Where(e => e.AccountStatus == AccountStatus.OnModeration)
            .Select(e => _mapper.Map<TravelAgentDto>(e))
            .ToArrayAsync();
    }
}
