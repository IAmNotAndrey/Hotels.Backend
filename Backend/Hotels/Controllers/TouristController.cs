namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TouristController : ControllerBase
{
    private readonly ITouristRepo _touristRepo;
    private readonly IGenericRepo<Tourist, string> _repo;
    private readonly IApplicationUserRepo _appUserRepo;

    public TouristController(ITouristRepo touristRepo, IGenericRepo<Tourist, string> repo, IApplicationUserRepo appUserRepo)
    {
        _touristRepo = touristRepo;
        _repo = repo;
        _appUserRepo = appUserRepo;
    }

    /// <summary>
    /// Get all Tourists
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TouristDto>>> Get()
    {
        return Ok(await _touristRepo.GetDtosIncludedAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TouristDto>> Get(string id)
    {
        return Ok(await _touristRepo.GetDtoIncludedAsync(id));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<IActionResult> Update(string id, [FromForm] TouristDtoB dtoB)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Tourist)} wasn't found.");
        }
        if (!await _appUserRepo.IsUserAllowedAsync(User, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _repo.UpdateAsync(id, dtoB);
        return Ok();
    }
}
