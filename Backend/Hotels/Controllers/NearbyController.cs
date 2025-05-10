namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class NearbyController : ApplicationControllerBase<Nearby, Guid, NearbyDto, NearbyDtoB>
{
    private readonly IApplicationUserService _appUserRepo;
    private readonly INearbyRepo _nearbyRepo;
    private readonly IGenericRepo<CountrySubject, Guid> _countrySubjectRepo;
    private readonly IGenericRepo<Partner, string> _partnerRepo;
    private readonly INearbyService _nearbyService;

    public NearbyController(IApplicationUserService appUserRepo,
                            IGenericRepo<Nearby, Guid> repo,
                            INearbyRepo nearbyRepo,
                            IGenericRepo<CountrySubject, Guid> countrySubjectRepo,
                            IGenericRepo<Partner, string> partnerRepo,
                            INearbyService nearbyService) : base(repo)
    {
        _appUserRepo = appUserRepo;
        _nearbyRepo = nearbyRepo;
        _countrySubjectRepo = countrySubjectRepo;
        _partnerRepo = partnerRepo;
        _nearbyService = nearbyService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NearbyDto>>> Get()
    {
        return Ok(await _nearbyRepo.GetDtosIncludedAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NearbyDto>> Get(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Nearby)} wasn't found.");
        }
        return Ok(await _nearbyRepo.GetDtoIncludedAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] NearbyDtoB dtoB)
    {
        if (!await _countrySubjectRepo.ExistsAsync(dtoB.CountrySubjectId))
        {
            return NotFound($"{nameof(CountrySubject)} wasn't found.");
        }
        Nearby nearby = await _repo.AddAsync(dtoB);
        return CreatedAtAction(nameof(Create), nearby.Id);
    }

    /// <summary>
    /// Connects 'Nearby' to 'Partner'
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Link([Required, FromForm] string partnerId, [Required, FromForm] Guid nearbyId)
    {
        if (!await _partnerRepo.ExistsAsync(partnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, partnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _repo.ExistsAsync(nearbyId))
        {
            return NotFound($"{nameof(Nearby)} wasn't found.");
        }
        try
        {
            await _nearbyService.LinkAsync(partnerId, nearbyId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Unlink([Required, FromForm] string partnerId, [Required, FromForm] Guid nearbyId)
    {
        if (!await _partnerRepo.ExistsAsync(partnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, partnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _repo.ExistsAsync(nearbyId))
        {
            return NotFound($"{nameof(Nearby)} wasn't found.");
        }
        try
        {
            await _nearbyService.UninkAsync(partnerId, nearbyId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPut("{nearbyId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid nearbyId, NearbyDtoB dtoB)
    {
        return await base.Update_(nearbyId, dtoB);
    }

    [HttpDelete("{nearbyId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete([Required] Guid nearbyId)
    {
        return await base.Delete_(nearbyId);
    }
}
