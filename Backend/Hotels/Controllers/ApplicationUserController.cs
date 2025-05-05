namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly IApplicationUserRepo _userRepo;
    private readonly IGenericRepo<ApplicationUser, string> _repo;

    public ApplicationUserController(IApplicationUserRepo userRepo, IGenericRepo<ApplicationUser, string> repo)
    {
        _userRepo = userRepo;
        _repo = repo;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeName([Required] string userId, [Required] string newName)
    {
        if (!await _userRepo.IsUserAllowedAsync(User, userId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        if (!await _repo.ExistsAsync(userId))
        {
            return NotFound($"{nameof(ApplicationUser)} wasn't found.");
        }
        await _userRepo.ChangeNameAsync(userId, newName);
        return Ok();
    }
}
