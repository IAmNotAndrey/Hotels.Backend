namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ISmsSender _smsSender;
    private readonly UserManager<Tourist> _touristUM;
    private readonly SignInManager<Tourist> _touristSIM;
    private readonly IAdminRepo _adminRepo;
    private readonly UserManager<Admin> _adminUM;
    private readonly SignInManager<Admin> _adminSIM;

    public AccountController(ISmsSender smsSender,
                             UserManager<Tourist> touristUM,
                             SignInManager<Tourist> touristSIM,
                             IAdminRepo adminRepo,
                             UserManager<Admin> adminUM,
                             SignInManager<Admin> adminSIM)
    {
        _smsSender = smsSender;
        _touristUM = touristUM;
        _touristSIM = touristSIM;
        _adminRepo = adminRepo;
        _adminUM = adminUM;
        _adminSIM = adminSIM;
    }

    /// <summary>
    /// Get 'Id' of the current authorized user.
    /// </summary>
    [HttpGet]
    [Authorize]
    public ActionResult<string> GetCurrentUserId()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(userId);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return Ok();
    }


    #region Tourist

    /// <summary>
    /// Creates a new tourist if he wasn't registered previously. Sends a confirmation SMS anyway.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> TouristSendPhoneToRegisterOrLogin([Required, Phone] string number)
    {
        Tourist? tourist = await _touristUM.FindByNameAsync(number);
        // If an user wasn't found in DB then create it.
        if (tourist == null)
        {
            tourist = new()
            {
                UserName = number,
                AccountStatus = AccountStatus.Inactive
            };
            await _touristUM.CreateAsync(tourist);
        }

        // Get a phone number of the requester.
        Tourist? requester = await _touristUM.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        // If the requester try to sign in an account he is already singed in.
        if (User.Identity?.IsAuthenticated == true && requester?.UserName == number)
        {
            return Conflict("You are already authorized.");
        }

        // Create a verify token (code).
        string token = await _touristUM.GenerateChangePhoneNumberTokenAsync(tourist, number);
        // Send SMS.
        await _smsSender.SendSmsAsync(number, $"Your verification code: {token}");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> TouristLogin(
        [Required, Phone] string number,
        [Required] string code)
    {
        Tourist? tourist = await _touristUM.FindByNameAsync(number);
        if (tourist == null)
        {
            return NotFound($"'{nameof(Tourist)}' wasn't found by number '{number}'.");
        }

        var isTokenValid = await _touristUM.VerifyChangePhoneNumberTokenAsync(tourist, code, number);
        if (!isTokenValid)
        {
            return BadRequest($"Verification code '{code}' is invalid.");
        }

        // After successfull phone confirmation.
        tourist.PhoneNumber = number;
        tourist.PhoneNumberConfirmed = true;
        tourist.AccountStatus = AccountStatus.Active;

        await _touristUM.UpdateAsync(tourist);
        await _touristSIM.SignInAsync(tourist, isPersistent: true);

        return Ok();
    }

#if DEBUG
    [HttpGet]
    public async Task<IActionResult> TouristLoginDebug([Required, Phone] string number)
    {
        Tourist? tourist = await _touristUM.FindByNameAsync(number);
        if (tourist == null)
        {
            return NotFound($"'{nameof(Tourist)}' wasn't found by number '{number}'.");
        }
        await _touristSIM.SignInAsync(tourist, isPersistent: true);

        return Ok();
    }
#endif

    #endregion

    #region Admin

    [HttpPost]
    public async Task<IActionResult> AdminLogin([Required, EmailAddress] string email, [Required] string password)
    {
        var admin = await _adminUM.FindByEmailAsync(email);
        if (admin == null)
        {
            return NotFound($"{nameof(Admin)} wasn't found.");
        }
        if (!await _adminSIM.CanSignInAsync(admin))
        {
            return BadRequest($"{nameof(Admin)} cannot be signed in.");
        }
        // Check password.
        var passwordValid = await _adminUM.CheckPasswordAsync(admin, password);
        if (!passwordValid)
        {
            return Unauthorized("Invalid credentials");
        }
        await _adminSIM.SignInAsync(admin, isPersistent: true);
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> AdminRegister([Required, EmailAddress] string email, [Required] string password)
    {
        await _adminRepo.CreateAsync(email, password);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> AdminDelete(string id)
    {
        await _adminRepo.DeleteAsync(id);
        return Ok();
    }

    #endregion
}
