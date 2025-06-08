using Hotels.PartnerReviews.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.PartnerReviews.Controllers;

[ApiController]
[Route("api/internal/v1/partner_reviews/[action]")]
public class PartnerReviewsInternalController(IReviewRepo reviewRepo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDtosIncluded()
    {
        return Ok(await reviewRepo.GetDtosIncludedAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDtoIncludedAsync(Guid id)
    {
        return Ok(await reviewRepo.GetDtoIncludedAsync(id));
    }

    [HttpGet("{partnerId}")]
    public async Task<IActionResult> GetDtosIncludedByPartner(string partnerId)
    {
        return Ok(await reviewRepo.GetDtosIncludedByPartnerAsync(partnerId));
    }

    [HttpGet("{touristId}")]
    public async Task<IActionResult> GetDtosIncludedByTourist(string touristId)
    {
        return Ok(await reviewRepo.GetDtosIncludedByTouristAsync(touristId));
    }
}
