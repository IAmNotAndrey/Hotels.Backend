namespace Hotels.Controllers.Comforts;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class SubobjectComfortController : ApplicationControllerBase<SubobjectComfort, Guid, SubobjectComfortDto, SubobjectComfortDtoB>
{
    private readonly IApplicationUserService _appUserRepo;
    private readonly IGenericRepo<Subobject, Guid> _subobjectRepo;
    private readonly ISubobjectComfortRepo _subobjectComfortRepo;

    public SubobjectComfortController(IApplicationUserService appUserRepo,
                                      IGenericRepo<Subobject, Guid> subobjectRepo,
                                      ISubobjectComfortRepo subobjectComfortRepo,
                                      IGenericRepo<SubobjectComfort, Guid> repo) : base(repo)
    {
        _appUserRepo = appUserRepo;
        _subobjectRepo = subobjectRepo;
        _subobjectComfortRepo = subobjectComfortRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubobjectComfortDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubobjectComfortDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] SubobjectComfortDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    /// <summary>
    /// Connects SubobjectComfort to Subobject
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Link([Required, FromForm] Guid subobjectId, [Required, FromForm] Guid subobjectComfortId)
    {
        if (!await _subobjectRepo.ExistsAsync(subobjectId))
        {
            return NotFound($"{nameof(Subobject)} wasn't found.");
        }
        Subobject subobject = await _subobjectRepo.GetByIdAsync(subobjectId);
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, subobject.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _repo.ExistsAsync(subobjectComfortId))
        {
            return NotFound($"{nameof(SubobjectComfort)} wasn't found.");
        }
        try
        {
            await _subobjectComfortRepo.LinkAsync(subobjectId, subobjectComfortId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Unlink([Required, FromForm] Guid subobjectId, [Required, FromForm] Guid subobjectComfortId)
    {
        if (!await _subobjectRepo.ExistsAsync(subobjectId))
        {
            return NotFound($"{nameof(Subobject)} wasn't found.");
        }
        Subobject subobject = (await _subobjectRepo.GetByIdOrDefaultAsync(subobjectId))!;
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, subobject.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _repo.ExistsAsync(subobjectComfortId))
        {
            return NotFound($"{nameof(SubobjectComfort)} wasn't found.");
        }
        try
        {
            await _subobjectComfortRepo.UnlinkAsync(subobjectId, subobjectComfortId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPut("{subobjectComfortId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid subobjectComfortId, SubobjectComfortDtoB dtoB)
    {
        return await base.Update_(subobjectComfortId, dtoB);
    }

    [HttpDelete("{subobjectComfortId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete(Guid subobjectComfortId)
    {
        return await base.Delete_(subobjectComfortId);
    }
}
