namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CafeSubscriptionController : ApplicationControllerBase<CafeSubscription, Guid, CafeSubscriptionDto, object>
{
    private readonly ICafeSubscriptionRepo _cafeSubRepo;
    private readonly IGenericRepo<Cafe, Guid> _cafeRepo;
    private readonly IGenericRepo<CafeTimeLimitedPaidService, Guid> _cafeTLPSRepo;

    public CafeSubscriptionController(ICafeSubscriptionRepo cafeSubRepo,
                                      IGenericRepo<Cafe, Guid> cafeRepo,
                                      IGenericRepo<CafeTimeLimitedPaidService, Guid> cafeTLPSRepo,
                                      IGenericRepo<CafeSubscription, Guid> repo) : base(repo)
    {
        _cafeSubRepo = cafeSubRepo;
        _cafeRepo = cafeRepo;
        _cafeTLPSRepo = cafeTLPSRepo;
    }

    [HttpGet("{cafeId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<IEnumerable<TravelAgentSubscriptionDto>>> GetByCafe(Guid cafeId)
    {
        if (!await _cafeRepo.ExistsAsync(cafeId))
        {
            return NotFound($"{nameof(Cafe)} wasn't found.");
        }
        var dtos = await _cafeSubRepo.GetDtosIncludedByCafeAsync(cafeId);
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([Required] Guid cafeId, [Required] Guid paidServiceId)
    {
        if (!await _cafeRepo.ExistsAsync(cafeId))
        {
            return NotFound($"{nameof(Cafe)} wasn't found.");
        }
        if (!await _cafeTLPSRepo.ExistsAsync(paidServiceId))
        {
            return NotFound($"{nameof(CafeTimeLimitedPaidService)} wasn't found.");
        }
        var sub = await _cafeSubRepo.CreateAsync(cafeId, paidServiceId);
        return CreatedAtAction(nameof(Create), sub.Id);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await base.Delete_(id);
    }
}
