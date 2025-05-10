namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TravelAgentSubscriptionController : ApplicationControllerBase<TravelAgentSubscription, Guid, TravelAgentSubscriptionDto, object>
{
    private readonly ITravelAgentSubscriptionRepo _taSubscriptionRepo;
    private readonly IApplicationUserService _appUserRepo;
    private readonly ITravelAgentSubscriptionService _travelAgentSubscriptionService;

    public TravelAgentSubscriptionController(ITravelAgentSubscriptionRepo taSubscriptionRepo,
                                             IApplicationUserService appUserRepo,
                                             IGenericRepo<TravelAgentSubscription, Guid> repo,
                                             ITravelAgentSubscriptionService travelAgentSubscriptionService) : base(repo)
    {
        _taSubscriptionRepo = taSubscriptionRepo;
        _appUserRepo = appUserRepo;
        _travelAgentSubscriptionService = travelAgentSubscriptionService;
    }

    [HttpGet("{travelAgentId}")]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<ActionResult<IEnumerable<TravelAgentSubscriptionDto>>> Get(string travelAgentId)
    {
        if (!await _appUserRepo.IsUserAllowedAsync(User, travelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        var dtos = await _taSubscriptionRepo.GetDtosIncludedByTravelAgent(travelAgentId);
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Create([Required] string travelAgentId, [Required] Guid paidServiceId)
    {
        if (!await _appUserRepo.IsUserAllowedAsync(User, travelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        var sub = await _travelAgentSubscriptionService.CreateAsync(travelAgentId, paidServiceId);
        return CreatedAtAction(nameof(Create), sub.Id);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(TravelAgentSubscription)} wasn't found.");
        }
        TravelAgentSubscription subscription = (await _repo.GetByIdOrDefaultAsync(id))!;
        if (!await _appUserRepo.IsUserAllowedAsync(User, subscription!.TravelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Delete_(id);
    }
}
