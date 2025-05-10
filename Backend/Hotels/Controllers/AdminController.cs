namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
[Authorize(Roles = nameof(Admin))]
public class AdminController : ControllerBase
{
    private readonly IAdminRepo _adminRepo;
    private readonly IGenericRepo<ApplicationUser, string> _appUserRepo;
    private readonly IAdminService _adminService;

    public AdminController(IAdminRepo adminRepo,
                           IGenericRepo<ApplicationUser, string> appUserRepo,
                           IAdminService adminService)
    {
        _adminRepo = adminRepo;
        _appUserRepo = appUserRepo;
        _adminService = adminService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartnerDto>>> GetPartnersOnModeration()
    {
        return Ok(await _adminRepo.GetPartnersOnModerationAsync());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TravelAgentDto>>> GetTravelAgentsOnModeration()
    {
        return Ok(await _adminRepo.GetTravelAgentsOnModerationAsync());

    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> ConfirmModeration(string userId)
    {
        if (!await _appUserRepo.ExistsAsync(userId))
        {
            return NotFound($"{nameof(ApplicationUser)} wasn't found.");
        }
        ApplicationUser user = await _appUserRepo.GetByIdAsync(userId);
        if (user.AccountStatus != AccountStatus.OnModeration)
        {
            return BadRequest($"{nameof(ApplicationUser)} is not {nameof(AccountStatus.OnModeration)}.");
        }
        await _adminService.ConfirmModerationAsync(userId);
        return Ok();
    }
}
