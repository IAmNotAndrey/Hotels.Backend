namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TourReviewController : ApplicationControllerBase<TourReview, Guid, TourReviewDto, TourReviewDtoB>
{
    private readonly IGenericRepo<Tour, Guid> _tourRepo;
    private readonly IApplicationUserService _appUserRepo;
    private readonly IGenericRepo<Tourist, string> _touristRepo;

    public TourReviewController(IGenericRepo<TourReview, Guid> repo,
                                IGenericRepo<Tour, Guid> tourRepo,
                                IApplicationUserService appUserRepo,
                                IGenericRepo<Tourist, string> touristRepo) : base(repo)
    {
        _tourRepo = tourRepo;
        _appUserRepo = appUserRepo;
        _touristRepo = touristRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourReviewDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TourReviewDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> Create([FromForm] TourReviewDtoB dtoB)
    {
        if (!await _tourRepo.ExistsAsync(dtoB.TourId))
        {
            return NotFound($"{nameof(Tour)} wasn't found.");
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
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(PartnerReview)} wasn't found.");
        }
        TourReview review = await _repo.GetByIdAsync(id);
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, review.TouristId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Delete_(id);
    }
}
