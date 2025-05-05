using Hotels.Application.Dtos.Reviews;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IReviewRepo
{
    Task<PartnerReviewDto> GetDtoIncludedAsync(Guid id);
    Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedAsync();
    Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByPartnerAsync(string partnerId);
    Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByTouristAsync(string touristId);
}
