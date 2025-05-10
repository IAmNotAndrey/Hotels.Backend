namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TravelAgentController : ControllerBase
{
    private readonly ITravelAgentRepo _travelAgentRepo;
    private readonly IApplicationUserService _appUserRepo;
    private readonly IGenericRepo<TravelAgent, string> _repo;

    public TravelAgentController(ITravelAgentRepo travelAgentRepo,
                                 IApplicationUserService appUserRepo,
                                 IGenericRepo<TravelAgent, string> repo)
    {
        _travelAgentRepo = travelAgentRepo;
        _appUserRepo = appUserRepo;
        _repo = repo;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<TravelAgentDto>>> GetByFilter([FromBody] TravelAgentFilter filter)
    {
        return Ok(await _travelAgentRepo.GetDtosIncludedAsync(filter));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TravelAgentDto>> Get(string id)
    {
        return Ok(await _travelAgentRepo.GetDtoIncludedAsync(id));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Update(string id, [FromForm] TravelAgentDtoB dtoB)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(TravelAgent)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _travelAgentRepo.UpdateAsync(id, dtoB);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(TravelAgent)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _repo.DeleteAsync(id);
        return Ok();
    }
}
