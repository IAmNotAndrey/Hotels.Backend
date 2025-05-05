namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class BathroomController : ApplicationControllerBase<Bathroom, Guid, BathroomDto, BathroomDtoB>
{
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IGenericRepo<Subobject, Guid> _subobjectRepo;
    private readonly IBathroomRepo _bathroomRepo;

    public BathroomController(IApplicationUserRepo appUserRepo,
                              IGenericRepo<Subobject, Guid> subobjectRepo,
                              IBathroomRepo bathroomRepo,
                              IGenericRepo<Bathroom, Guid> repo) : base(repo)
    {
        _appUserRepo = appUserRepo;
        _subobjectRepo = subobjectRepo;
        _bathroomRepo = bathroomRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BathroomDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BathroomDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] BathroomDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    /// <summary>
    /// Connects 'Bathroom' to 'Subobject'
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Link([Required, FromForm] Guid subobjectId, [Required, FromForm] Guid bathroomId)
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
        if (!await _repo.ExistsAsync(bathroomId))
        {
            return NotFound($"{nameof(Bathroom)} wasn't found.");
        }
        try
        {
            await _bathroomRepo.LinkAsync(subobjectId, bathroomId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Unlink([Required, FromForm] Guid subobjectId, [Required, FromForm] Guid bathroomId)
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
        if (!await _repo.ExistsAsync(bathroomId))
        {
            return NotFound($"{nameof(Bathroom)} wasn't found.");
        }
        try
        {
            await _bathroomRepo.UnlinkAsync(subobjectId, bathroomId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, [FromForm] BathroomDtoB dtoB)
    {
        return await base.Update_(id, dtoB);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete([Required] Guid id)
    {
        return await base.Delete_(id);
    }
}
