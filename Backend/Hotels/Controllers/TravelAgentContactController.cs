namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TravelAgentContactController : ApplicationControllerBase<ApplicationObjectContact, Guid, ApplicationObjectContactDto, ApplicationObjectContactDtoB>
{
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IGenericRepo<ApplicationObject, string> _appObjectRepo;

    public TravelAgentContactController(IApplicationUserRepo appUserRepo,
                                        IGenericRepo<ApplicationObject, string> appObjectRepo,
                                        IGenericRepo<ApplicationObjectContact, Guid> repo) : base(repo)
    {
        _appUserRepo = appUserRepo;
        _appObjectRepo = appObjectRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationObjectContactDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> Create([FromForm] ApplicationObjectContactDtoB dtoB)
    {
        if (!await _appObjectRepo.ExistsAsync(dtoB.ApplicationObjectId))
        {
            return NotFound($"{nameof(ApplicationObject)} wasn't found.");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.ApplicationObjectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ApplicationObjectContact? contact = await _repo.GetByIdOrDefaultAsync(id);
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, contact?.ApplicationObjectId ?? ""))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Delete_(id);
    }
}
