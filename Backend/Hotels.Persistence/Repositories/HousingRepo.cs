using AutoMapper;
using Hotels.Application.Dtos.Subobjects;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Hotels.Persistence.Repositories;

public class HousingRepo : IHousingRepo
{
    private readonly ApplicationContext _db;
    private readonly IGenericRepo<Housing, Guid> _repo;
    private readonly IMapper _mapper;
    private readonly IGenericRepo<Partner, string> _partnerRepo;

    public ImmutableHashSet<string> SubobjectChildrenTypeNames { get; } = ImmutableHashSet.CreateRange<string>(
        [nameof(Housing), nameof(Room)]
    );

    public HousingRepo(ApplicationContext db, IGenericRepo<Housing, Guid> repo, IMapper mapper, IGenericRepo<Partner, string> partnerRepo)
    {
        _db = db;
        _repo = repo;
        _mapper = mapper;
        _partnerRepo = partnerRepo;
    }

    public async Task<IEnumerable<HousingDto>> GetDtosIncludedAsync()
    {
        var housings = await IncludeHousingRelations(_repo.Entities)
            .Select(e => _mapper.Map<HousingDto>(e))
            .ToArrayAsync();

        return housings;
    }

    public async Task<HousingDto> GetDtoIncludedAsync(Guid id)
    {
        var housing = await IncludeHousingRelations(_db.Housings)
            .FirstAsync(e => e.Id == id);

        return _mapper.Map<HousingDto>(housing);
    }

    public async Task<IEnumerable<HousingDto>> GetDtosIncludedByPartnerAsync(string partnerId)
    {
        var housings = await IncludeHousingRelations(_repo.Entities)
            .Where(e => e.PartnerId == partnerId)
            .Select(e => _mapper.Map<HousingDto>(e))
            .ToArrayAsync();

        return housings;
    }

    private static IQueryable<Housing> IncludeHousingRelations(IQueryable<Housing> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(h => h.Comforts)
            .Include(h => h.WeekRate)
            .Include(h => h.Feeds)
            .Include(h => h.Bookings)
            .Include(h => h.ImageLinks)
            .Include(h => h.Partner)
            .Include(h => h.Toilets)
            .Include(h => h.Bathrooms)
            .Include(h => h.Type);
    }
}
