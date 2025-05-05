namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    // TODO

    public EmailController(IEmailSender emailSender, UserManager<ApplicationUser> userManager)
    {
        _emailSender = emailSender;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeUserEmail([Required, EmailAddress] string email)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        string token = await _userManager.GenerateChangeEmailTokenAsync(user, email);

        // Создаём URL для подтверждения смены email
        var callbackUrl = Url.Action(
            nameof(ConfirmEmailChange),
            "Email",
            new { userId, email, code = token },
            protocol: HttpContext.Request.Scheme);

        // Отправляем email с подтверждением
        await _emailSender.SendEmailAsync(
            email,
            "Confirm your email change",
            $"Please confirm your email change by <a href='{callbackUrl}'>clicking here</a>.");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmailChange([Required] string userId, [Required] string email, [Required] string code)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"{nameof(ApplicationUser)} hasn't been found by id '{userId}'");
        }

        var res = await _userManager.ChangeEmailAsync(user, email, code);
        if (!res.Succeeded)
        {
            return BadRequest(res);
        }

        return Ok();
    }
}
