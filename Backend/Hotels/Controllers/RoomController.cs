namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class RoomController : ApplicationControllerBase<Room, Guid, RoomDto, RoomDtoB>
{
    private readonly IRoomRepo _roomRepo;
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IGenericRepo<Partner, string> _partnerRepo;

    public RoomController(IRoomRepo roomRepo,
                          IApplicationUserRepo appUserRepo,
                          IGenericRepo<Partner, string> partnerRepo,
                          IGenericRepo<Room, Guid> repo) : base(repo)
    {
        _roomRepo = roomRepo;
        _appUserRepo = appUserRepo;
        _partnerRepo = partnerRepo;
    }

    // TODO
    [HttpGet("{partnerId}")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetByPartner(string partnerId)
    {
        if (!await _partnerRepo.ExistsAsync(partnerId))
        {
            return NotFound($"{nameof(Partner)} wasn't found.");
        }
        IEnumerable<RoomDto> dtos = await _roomRepo.GetDtosIncludedByPartnerAsync(partnerId);
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<ActionResult<Guid>> Create([FromForm] RoomDtoB dtoB)
    {
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] RoomDtoB dtoB)
    {
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, dtoB.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Update_(id, dtoB);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Room)} wasn't found.");
        }
        Room room = (await _repo.GetByIdOrDefaultAsync(id))!;
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, room.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        return await base.Delete_(id);
    }
}
