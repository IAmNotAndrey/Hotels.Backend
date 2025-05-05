using AutoMapper;
using Hotels.Application.Dtos.Subscriptions;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class TravelAgentSubscriptionRepo : ITravelAgentSubscriptionRepo
{
    private readonly IMapper _mapper;
    private readonly IGenericRepo<TravelAgentSubscription, Guid> _repo;

    public TravelAgentSubscriptionRepo(IMapper mapper, IGenericRepo<TravelAgentSubscription, Guid> repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<IEnumerable<TravelAgentSubscriptionDto>> GetDtosIncludedByTravelAgent(string travelAgentId)
    {
        IEnumerable<TravelAgentSubscriptionDto> dtos = await IncludeRelations(_repo.Entities)
            .Where(e => e.TravelAgentId == travelAgentId)
            .Select(e => _mapper.Map<TravelAgentSubscriptionDto>(e))
            .ToArrayAsync();
        return dtos;
    }

    public async Task<TravelAgentSubscription> CreateAsync(string travelAgentId, Guid paidServiceId)
    {
        TravelAgentSubscription sub = new() { TravelAgentId = travelAgentId, PaidServiceId = paidServiceId };
        return await _repo.AddAsync(sub);
    }

    private static IQueryable<TravelAgentSubscription> IncludeRelations(IQueryable<TravelAgentSubscription> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.PaidService);

    }
}
