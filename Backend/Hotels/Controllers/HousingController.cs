namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class HousingController : ApplicationControllerBase<Housing, Guid, HousingDto, HousingDtoB>
{
    private readonly IHousingRepo _housingRepo;
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IGenericRepo<Partner, string> _partnerRepo;

    public HousingController(IHousingRepo housingRepo,
                             IApplicationUserRepo appUserRepo,
                             IGenericRepo<Housing, Guid> repo,
                             IGenericRepo<Partner, string> partnerRepo) : base(repo)
    {
        _housingRepo = housingRepo;
        _appUserRepo = appUserRepo;
        _partnerRepo = partnerRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HousingDto>>> Get()
    {
        return Ok(await _housingRepo.GetDtosIncludedAsync());
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HousingDto>> Get(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Housing)} wasn't found.");
        }
        return Ok(await _housingRepo.GetDtoIncludedAsync(id));
    }

    [HttpGet("{partnerId}")]
    public async Task<ActionResult<IEnumerable<HousingDto>>> GetByPartner(string partnerId)
    {
        return Ok(await _housingRepo.GetDtosIncludedByPartnerAsync(partnerId));
    }

    // TODO
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> Create([FromForm] HousingDtoB dtoB)
    {
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _partnerRepo.ExistsAsync(dtoB.PartnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found");
        }
        Housing housing = await _repo.AddAsync(dtoB);
        return CreatedAtAction(nameof(Create), housing.Id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] HousingDtoB dtoB)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Housing)} wasn't found");
        }
        // Does the requester do an allowed operation?
        Housing housing = (await _repo.GetByIdOrDefaultAsync(id))!;
        if (!await _appUserRepo.IsUserAllowedAsync(User, housing.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _repo.UpdateAsync(id, dtoB);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Housing)} wasn't found");
        }
        // Does the requester do an allowed operation?
        Housing housing = (await _repo.GetByIdOrDefaultAsync(id))!;
        if (!await _appUserRepo.IsUserAllowedAsync(User, housing.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        await _repo.DeleteAsync(id);
        return Ok();
    }
}
