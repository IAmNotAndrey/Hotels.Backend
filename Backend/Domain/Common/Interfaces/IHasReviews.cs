using Hotels.Domain.Entities.Reviews;

namespace Hotels.Domain.Common.Interfaces;

public interface IHasReviews<TReview> where TReview : Review
{
    ICollection<TReview> Reviews { get; }
}
