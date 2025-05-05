using AutoMapper;
using Hotels.Application.Dtos.Users;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Hotels.Presentation.DtoBs.Users;
using Hotels.Presentation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class TravelAgentRepo : ITravelAgentRepo
{
    private readonly IMapper _mapper;
    private readonly IGenericRepo<TravelAgent, string> _repo;
    private readonly ApplicationContext _db;

    public TravelAgentRepo(IMapper mapper, IGenericRepo<TravelAgent, string> repo, ApplicationContext db)
    {
        _mapper = mapper;
        _repo = repo;
        _db = db;
    }

    public async Task<TravelAgentDto> GetDtoIncludedAsync(string id)
    {
        TravelAgent travelAgent = await IncludeRelations(_repo.Entities)
           .FirstAsync(e => e.Id == id);
        return _mapper.Map<TravelAgentDto>(travelAgent);
    }

    public async Task<IEnumerable<TravelAgentDto>> GetDtosIncludedAsync(IFilterModel<TravelAgent> filter)
    {
        return await IncludeRelations(_repo.Entities)
            .Where(filter.FilterExpression)
            .Select(e => _mapper.Map<TravelAgentDto>(e))
            .ToArrayAsync();
    }

    public async Task<IEnumerable<TravelAgentDto>> GetDtosIncludedAsync()
    {
        return await IncludeRelations(_repo.Entities)
            .Select(e => _mapper.Map<TravelAgentDto>(e))
            .ToArrayAsync();
    }

    public async Task UpdateAsync(string id, TravelAgentDtoB dtoB)
    {
        await _repo.UpdateAsync(id, dtoB);

        TravelAgent travelAgent = await _db.TravelAgents.FirstAsync(e => e.Id == id);
        // Validate changes
        bool isValidForModeration = IsValidForModeration(travelAgent, out _);
        if (isValidForModeration)
        {
            travelAgent.AccountStatus = AccountStatus.Active;
        }
        else
        {
            travelAgent.AccountStatus = AccountStatus.Inactive;
        }
        await _db.SaveChangesAsync();
    }

    private static bool IsValidForModeration(TravelAgent travelAgent, out List<string> validationErrors)
    {
        validationErrors = [];

        if (string.IsNullOrWhiteSpace(travelAgent.WebsiteUrl))
        {
            validationErrors.Add($"{nameof(travelAgent.WebsiteUrl)} is required.");
        }
        if (travelAgent.CountrySubjectId == null)
        {
            validationErrors.Add($"{nameof(travelAgent.CountrySubjectId)} is required.");
        }
        if (string.IsNullOrWhiteSpace(travelAgent.Description))
        {
            validationErrors.Add($"{nameof(travelAgent.Description)} is required.");
        }
        if (string.IsNullOrWhiteSpace(travelAgent.Address))
        {
            validationErrors.Add($"{nameof(travelAgent.Address)} is required.");
        }
        if (string.IsNullOrWhiteSpace(travelAgent.Coordinates))
        {
            validationErrors.Add($"{nameof(travelAgent.Coordinates)} is required.");
        }
        if (string.IsNullOrWhiteSpace(travelAgent.Name))
        {
            validationErrors.Add($"{nameof(travelAgent.Name)} is required.");
        }

        return validationErrors.Count == 0;
    }

    private static IQueryable<TravelAgent> IncludeRelations(IQueryable<TravelAgent> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.LogoLink)
            .Include(e => e.Tours)
                .ThenInclude(t => t.ImageLinks)
            .Include(e => e.CountrySubject)
            .Include(e => e.ImageLinks)
            .Include(e => e.Contacts);
    }
}
