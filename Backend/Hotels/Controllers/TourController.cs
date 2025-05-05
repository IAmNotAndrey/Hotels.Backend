namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TourController : ApplicationControllerBase<Tour, Guid, TourDto, TourDtoB>
{
    private readonly ITourRepo _tourRepo;
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IGenericRepo<TravelAgent, string> _travelAgentRepo;
    private readonly IGenericRepo<CountrySubject, Guid> _countrySubjectRepo;

    public TourController(ITourRepo tourRepo,
                          IApplicationUserRepo appUserRepo,
                          IGenericRepo<TravelAgent, string> travelAgentRepo,
                          IGenericRepo<CountrySubject, Guid> countrySubjectRepo,
                          IGenericRepo<Tour, Guid> repo) : base(repo)
    {
        _tourRepo = tourRepo;
        _appUserRepo = appUserRepo;
        _travelAgentRepo = travelAgentRepo;
        _countrySubjectRepo = countrySubjectRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourDto>>> Get()
    {
        return Ok(await _tourRepo.GetDtosIncludedAsync());
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<TourDto>>> GetByFilter([FromBody] TourFilter filter)
    {
        return Ok(await _tourRepo.GetDtosIncludedAsync(filter));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TourDto>> Get(Guid id)
    {
        return Ok(await _tourRepo.GetDtoIncludedAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> Create([FromForm] TourDtoB dtoB)
    {
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.TravelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _travelAgentRepo.ExistsAsync(dtoB.TravelAgentId))
        {
            return NotFound($"'{nameof(TravelAgent)}' wasn't found.");
        }
        if (!await _countrySubjectRepo.ExistsAsync(dtoB.CountrySubjectId))
        {
            return NotFound($"'{nameof(CountrySubject)}' wasn't found.");
        }
        Tour tour = await _repo.AddAsync(dtoB);
        return CreatedAtAction(nameof(Create), tour.Id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] TourDtoB dtoB)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"'{nameof(Tour)}' wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.TravelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _travelAgentRepo.ExistsAsync(dtoB.TravelAgentId))
        {
            return NotFound($"'{nameof(TravelAgent)}' wasn't found.");
        }
        if (!await _countrySubjectRepo.ExistsAsync(dtoB.CountrySubjectId))
        {
            return NotFound($"'{nameof(CountrySubject)}' wasn't found.");
        }
        await _repo.UpdateAsync(id, dtoB);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"'{nameof(Tour)}' wasn't found.");
        }
        Tour tour = (await _repo.GetByIdOrDefaultAsync(id))!;
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, tour.TravelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _repo.DeleteAsync(id);
        return Ok();
    }
}
