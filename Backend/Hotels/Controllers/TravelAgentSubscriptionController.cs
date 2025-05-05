namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TravelAgentSubscriptionController : ApplicationControllerBase<TravelAgentSubscription, Guid, TravelAgentSubscriptionDto, object>
{
    private readonly ITravelAgentSubscriptionRepo _taSubscriptionRepo;
    private readonly IApplicationUserRepo _appUserRepo;

    public TravelAgentSubscriptionController(ITravelAgentSubscriptionRepo taSubscriptionRepo,
                                             IApplicationUserRepo appUserRepo,
                                             IGenericRepo<TravelAgentSubscription, Guid> repo) : base(repo)
    {
        _taSubscriptionRepo = taSubscriptionRepo;
        _appUserRepo = appUserRepo;
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
        var sub = await _taSubscriptionRepo.CreateAsync(travelAgentId, paidServiceId);
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
