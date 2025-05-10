using AutoMapper;
using Hotels.Application.Dtos.Users;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class AdminRepo : IAdminRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;

    public AdminRepo(ApplicationContext db,
                     IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
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
