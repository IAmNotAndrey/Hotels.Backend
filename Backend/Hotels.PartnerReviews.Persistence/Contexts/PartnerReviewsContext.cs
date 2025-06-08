using Hotels.Domain.Entities.Reviews;
using Microsoft.EntityFrameworkCore;

namespace Hotels.PartnerReviews.Persistence.Contexts;

public class PartnerReviewsContext : DbContext
{
    public virtual DbSet<PartnerReview> PartnerReviews { get; init; }

    public PartnerReviewsContext(DbContextOptions<PartnerReviewsContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
