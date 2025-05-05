namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class WeekRateController : ApplicationControllerBase<SubobjectWeekRate, Guid, SubobjectWeekRateDto, SubobjectWeekRateDtoB>
{
    private readonly ApplicationContext _db;
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IWeekRateRepo _weekRateRepo;
    private readonly IGenericRepo<Subobject, Guid> _subobjectRepo;

    public WeekRateController(ApplicationContext db,
                              IApplicationUserRepo appUserRepo,
                              IWeekRateRepo weekRateRepo,
                              IGenericRepo<Subobject, Guid> subobjectRepo,
                              IGenericRepo<SubobjectWeekRate, Guid> repo) : base(repo)
    {
        _db = db;
        _appUserRepo = appUserRepo;
        _weekRateRepo = weekRateRepo;
        _subobjectRepo = subobjectRepo;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubobjectWeekRateDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> CreateSubobjectWeekRate([FromForm] SubobjectWeekRateDtoB dtoB)
    {
        if (!await _subobjectRepo.ExistsAsync(dtoB.SubobjectId))
        {
            return NotFound($"{nameof(Subobject)} wasn't found.");
        }
        Subobject subobject = (await _subobjectRepo.GetByIdOrDefaultAsync(dtoB.SubobjectId))!;
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, subobject.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        SubobjectWeekRate weekRate = await _weekRateRepo.CreateAsync(dtoB);
        return CreatedAtAction(nameof(CreateSubobjectWeekRate), weekRate.Id);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> DeleteSubobjectWeekRate(Guid id)
    {
        Subobject subobject = await _db.Subobjects.AsNoTracking().FirstAsync(e => e.WeekRate != null && e.WeekRate.Id == id);
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, subobject.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Delete_(id);
    }
}
