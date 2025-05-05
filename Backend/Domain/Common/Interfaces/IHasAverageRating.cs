using Hotels.Domain.Entities.Reviews;

namespace Hotels.Domain.Common.Interfaces;

public interface IHasAverageRating<TReview> : IHasReviews<TReview> where TReview : Review
{
    float? AverageRating { get; }
}
