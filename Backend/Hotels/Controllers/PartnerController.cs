namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class PartnerController : ControllerBase
{
    private const string NotFoundText = $"{nameof(Partner)} wasn't found.";

    private readonly UserManager<Partner> _userManager;
    private readonly IPartnerRepo _partnerRepo;
    private readonly IApplicationUserService _appUserRepo;
    private readonly IGenericRepo<Partner, string> _partnerGenRepo;

    public PartnerController(UserManager<Partner> userManager,
                             IPartnerRepo partnerRepo,
                             IApplicationUserService appUserRepo,
                             IGenericRepo<Partner, string> partnerGenRepo)
    {
        _userManager = userManager;
        _partnerRepo = partnerRepo;
        _appUserRepo = appUserRepo;
        _partnerGenRepo = partnerGenRepo;
    }

    /// <summary>
    /// Get 'Admin's by a special admin's filter
    /// </summary>
    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<IEnumerable<PartnerDto>>> GetByFilterByAdmin([FromBody] PartnerFilter_ForAdminUse<SubobjectFilter> filter)
    {
        return Ok(await _partnerRepo.GetDtosIncludedByFilterAsync(filter));
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<PartnerDto>>> GetByFilter([FromBody] PartnerFilter<SubobjectFilter> filter)
    {
        return Ok(await _partnerRepo.GetDtosIncludedByFilterAsync(filter));
    }

    /// <summary>
    /// Get 'Partner' by 'Id'
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PartnerDto>> Get(string id)
    {
        if (!await _partnerGenRepo.ExistsAsync(id))
        {
            return NotFound(NotFoundText);
        }
        return Ok(await _partnerRepo.GetDtoIncludedAsync(id));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Update(string id, [FromForm] PartnerDtoB dtoB)
    {
        if (!await _partnerGenRepo.ExistsAsync(id))
        {
            return NotFound(NotFoundText);
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _partnerRepo.UpdateAsync(id, dtoB);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!await _partnerGenRepo.ExistsAsync(id))
        {
            return NotFound(NotFoundText);
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _partnerGenRepo.DeleteAsync(id);
        return Ok();
    }

    /// <summary>
    /// Send account to moderation.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> VerifyAccount(string id)
    {
        if (!await _partnerGenRepo.ExistsAsync(id))
        {
            return NotFound(NotFoundText);
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        // Validation
        if (!_partnerRepo.IsValidForModeration(await _partnerRepo.GetIncludedAsync(id), out var validationErrors))
        {
            return BadRequest(new { Errors = validationErrors });
        }
        await _partnerRepo.SetToModerateAsync(id);
        return Ok();
    }
}
