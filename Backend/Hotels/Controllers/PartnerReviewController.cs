namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class PartnerReviewController : ApplicationControllerBase<PartnerReview, Guid, PartnerReviewDto, PartnerReviewDtoB>
{
    private readonly IReviewRepo _reviewRepo;
    private readonly IApplicationUserService _appUserRepo;
    private readonly IGenericRepo<Partner, string> _partnerRepo;
    private readonly IGenericRepo<Tourist, string> _touristRepo;

    public PartnerReviewController(IReviewRepo reviewRepo,
                            IApplicationUserService appUserRepo,
                            IGenericRepo<Partner, string> partnerRepo,
                            IGenericRepo<Tourist, string> touristRepo,
                            IGenericRepo<PartnerReview, Guid> repo) : base(repo)
    {
        _reviewRepo = reviewRepo;
        _appUserRepo = appUserRepo;
        _partnerRepo = partnerRepo;
        _touristRepo = touristRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartnerReviewDto>>> Get()
    {
        return Ok(await _reviewRepo.GetDtosIncludedAsync());
    }

    /// <summary>
    /// Get 'Review' by 'Id'
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PartnerReviewDto>> Get(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(PartnerReview)} wasn't found.");
        }
        return Ok(await _reviewRepo.GetDtoIncludedAsync(id));
    }

    /// <summary>
    /// Get all 'Partner's reviews
    /// </summary>
    [HttpGet("{partnerId}")]
    public async Task<ActionResult<IEnumerable<PartnerReviewDto>>> GetPartnerReviews(string partnerId)
    {
        return Ok(await _reviewRepo.GetDtosIncludedByPartnerAsync(partnerId));
    }

    /// <summary>
    /// Get all 'PartnerReview's made by 'Tourist'
    /// </summary>
    [HttpGet("{touristId}")]
    public async Task<ActionResult<IEnumerable<PartnerReviewDto>>> GetTouristReviews(string touristId)
    {
        return Ok(await _reviewRepo.GetDtosIncludedByTouristAsync(touristId));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> Create([FromForm] PartnerReviewDtoB dtoB)
    {
        if (!await _partnerRepo.ExistsAsync(dtoB.PartnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found.");
        }
        if (!await _touristRepo.ExistsAsync(dtoB.TouristId))
        {
            return NotFound($"{nameof(Tourist)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.TouristId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await Create_(dtoB, nameof(Create));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(PartnerReview)} wasn't found.");
        }
        PartnerReview review = await _repo.GetByIdAsync(id);
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, review.TouristId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await Delete_(id);
    }
}