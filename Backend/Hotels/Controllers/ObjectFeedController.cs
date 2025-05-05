namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class ObjectFeedController : ApplicationControllerBase<ObjectFeed, Guid, ObjectFeedDto, ObjectFeedDtoB>
{
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IGenericRepo<Partner, string> _partnerRepo;
    private readonly IObjectFeedRepo _objectFeedController;

    public ObjectFeedController(IApplicationUserRepo appUserRepo,
                                IGenericRepo<Partner, string> partnerRepo,
                                IObjectFeedRepo objectFeedController,
                                IGenericRepo<ObjectFeed, Guid> repo) : base(repo)
    {
        _appUserRepo = appUserRepo;
        _partnerRepo = partnerRepo;
        _objectFeedController = objectFeedController;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjectFeedDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ObjectFeedDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] ObjectFeedDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    // TODO
    /// <summary>
    /// Conntects 'ObjectFeed' to 'Partner'
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Link([Required, FromForm] string partnerId, [Required, FromForm] Guid feedId)
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
        if (!await _repo.ExistsAsync(feedId))
        {
            return NotFound($"{nameof(ObjectFeed)} wasn't found.");
        }
        try
        {
            await _objectFeedController.LinkAsync(partnerId, feedId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Unlink([Required, FromForm] string partnerId, [Required, FromForm] Guid feedId)
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
        if (!await _repo.ExistsAsync(feedId))
        {
            return NotFound($"{nameof(ObjectFeed)} wasn't found.");
        }
        try
        {
            await _objectFeedController.UnlinkAsync(partnerId, feedId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, ObjectFeedDtoB dtoB)
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
