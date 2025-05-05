using AutoMapper;
using Hotels.Application.Dtos.Reviews;
using Hotels.Domain.Entities.Reviews;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class ReviewRepo : IReviewRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;
    private readonly IGenericRepo<PartnerReview, Guid> _repo;

    public ReviewRepo(ApplicationContext db, IMapper mapper, IGenericRepo<PartnerReview, Guid> repo)
    {
        _db = db;
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PartnerReviewDto> GetDtoIncludedAsync(Guid id)
    {
        PartnerReview review = await IncludeRelations(_repo.Entities).FirstAsync(e => e.Id == id);
        return _mapper.Map<PartnerReviewDto>(review);
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedAsync()
    {
        return await IncludeRelations(_repo.Entities)
            .Select(e => _mapper.Map<PartnerReviewDto>(e))
            .ToArrayAsync();
    }

    private IEnumerable<PartnerReviewDto> GetMapped(IEnumerable<PartnerReview> reviews)
    {
        return reviews.Select(e => _mapper.Map<PartnerReviewDto>(e));
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByPartnerAsync(string partnerId)
    {
        IEnumerable<PartnerReview> reviews = await IncludeRelations(_repo.Entities)
            .Where(e => e.PartnerId == partnerId)
            .ToArrayAsync();
        IEnumerable<PartnerReviewDto> dtos = GetMapped(reviews);
        return dtos;
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByTouristAsync(string touristId)
    {
        IEnumerable<PartnerReview> reviews = await IncludeRelations(_repo.Entities)
            .Where(e => e.TouristId == touristId)
            .ToArrayAsync();
        IEnumerable<PartnerReviewDto> dtos = GetMapped(reviews);
        return dtos;
    }

    private static IQueryable<PartnerReview> IncludeRelations(IQueryable<PartnerReview> query)
    {
        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.ImageLinks);
    }
}
