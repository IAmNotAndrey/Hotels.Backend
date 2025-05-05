namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CafeTimeLimitedPaidServiceController : ApplicationControllerBase<CafeTimeLimitedPaidService, Guid, CafeTimeLimitedPaidServiceDto, CafeTimeLimitedPaidServiceDtoB>
{
    public CafeTimeLimitedPaidServiceController(IGenericRepo<CafeTimeLimitedPaidService, Guid> repo) : base(repo) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CafeTimeLimitedPaidServiceDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CafeTimeLimitedPaidServiceDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create(CafeTimeLimitedPaidServiceDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, CafeTimeLimitedPaidServiceDtoB dtoB)
    {
        return await base.Update_(id, dtoB);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await base.Delete_(id);
    }
}
