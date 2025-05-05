namespace Hotels.Controllers.Comforts;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class ObjectComfortController : ApplicationControllerBase<ObjectComfort, Guid, ObjectComfortDto, ObjectComfortDtoB>
{
    private readonly IGenericRepo<Partner, string> _partnerRepo;
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IObjectComfortRepo _objectComfortRepo;

    public ObjectComfortController(IGenericRepo<Partner, string> partnerRepo,
                                   IApplicationUserRepo appUserRepo,
                                   IObjectComfortRepo objectComfortRepo,
                                   IGenericRepo<ObjectComfort, Guid> repo) : base(repo)
    {
        _partnerRepo = partnerRepo;
        _appUserRepo = appUserRepo;
        _objectComfortRepo = objectComfortRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectComfortDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ObjectComfortDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] ObjectComfortDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    /// <summary>
    /// Connects ObjectComfort to Partner
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Link([Required, FromForm] string partnerId, [Required, FromForm] Guid objectComfortId)
    {
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, partnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _partnerRepo.ExistsAsync(partnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found.");
        }
        if (!await _repo.ExistsAsync(objectComfortId))
        {
            return NotFound($"{nameof(ObjectComfort)} wasn't found.");
        }
        try
        {
            await _objectComfortRepo.LinkAsync(partnerId, objectComfortId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Unlink([Required, FromForm] string partnerId, [Required, FromForm] Guid objectComfortId)
    {
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, partnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _partnerRepo.ExistsAsync(partnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found.");
        }
        if (!await _repo.ExistsAsync(objectComfortId))
        {
            return NotFound($"{nameof(ObjectComfort)} wasn't found.");
        }
        try
        {
            await _objectComfortRepo.UnlinkAsync(partnerId, objectComfortId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, ObjectComfortDtoB dtoB)
    {
        return await base.Update_(id, dtoB);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await base.Delete_(id);
    }
}
