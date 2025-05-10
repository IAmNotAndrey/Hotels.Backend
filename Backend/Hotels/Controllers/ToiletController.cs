namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class ToiletController : ApplicationControllerBase<Toilet, Guid, ToiletDto, ToiletDtoB>
{
    private readonly IApplicationUserService _appUserRepo;
    private readonly IToiletRepo _toiletRepo;
    private readonly IGenericRepo<Subobject, Guid> _subobjectRepo;

    public ToiletController(IApplicationUserService appUserRepo,
                            IToiletRepo toiletRepo,
                            IGenericRepo<Subobject, Guid> subobjectRepo,
                            IGenericRepo<Toilet, Guid> repo) : base(repo)
    {
        _appUserRepo = appUserRepo;
        _toiletRepo = toiletRepo;
        _subobjectRepo = subobjectRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToiletDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ToiletDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] ToiletDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    /// <summary>
    /// Connects 'Toilet' to 'Subobject'
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Link([Required, FromForm] Guid subobjectId, [Required, FromForm] Guid toiletId)
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
        if (!await _repo.ExistsAsync(toiletId))
        {
            return NotFound($"{nameof(Toilet)} wasn't found.");
        }
        await _toiletRepo.LinkAsync(subobjectId, toiletId);
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Unlink([Required, FromForm] Guid subobjectId, [Required, FromForm] Guid toiletId)
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
        if (!await _repo.ExistsAsync(toiletId))
        {
            return NotFound($"{nameof(Toilet)} wasn't found.");
        }
        try
        {
            await _toiletRepo.UnlinkAsync(subobjectId, toiletId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(nameof(Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await base.Delete_(id);
    }
}
