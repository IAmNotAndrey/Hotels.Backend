using AutoMapper;
using Hotels.Application.Dtos.Reviews;
using Hotels.Domain.Entities.Reviews;
using Hotels.PartnerReviews.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.PartnerReviews.Persistence.Repositories;

public class ReviewRepo(IMapper mapper, IGenericRepo<PartnerReview, Guid> repo) : IReviewRepo
{
    public async Task<PartnerReviewDto> GetDtoIncludedAsync(Guid id)
    {
        PartnerReview review = await IncludeRelations(repo.Entities).FirstAsync(e => e.Id == id);
        return mapper.Map<PartnerReviewDto>(review);
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedAsync()
    {
        return await IncludeRelations(repo.Entities)
            .Select(e => mapper.Map<PartnerReviewDto>(e))
            .ToArrayAsync();
    }

    private IEnumerable<PartnerReviewDto> GetMapped(IEnumerable<PartnerReview> reviews)
    {
        return reviews.Select(e => mapper.Map<PartnerReviewDto>(e));
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByPartnerAsync(string partnerId)
    {
        IEnumerable<PartnerReview> reviews = await IncludeRelations(repo.Entities)
            .Where(e => e.PartnerId == partnerId)
            .ToArrayAsync();
        IEnumerable<PartnerReviewDto> dtos = GetMapped(reviews);
        return dtos;
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByTouristAsync(string touristId)
    {
        IEnumerable<PartnerReview> reviews = await IncludeRelations(repo.Entities)
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
