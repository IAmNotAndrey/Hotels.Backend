using Hotels.Domain.Entities.Reviews;

namespace Hotels.Domain.Entities.StaticFiles;

public class ReviewImageLink : TitledImageLink
{
    public required Guid ReviewId { get; set; }
    public PartnerReview Review { get; set; } = null!;
}
