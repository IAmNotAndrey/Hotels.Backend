namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TravelAgentTimeLimitedPaidServiceController : ApplicationControllerBase<TravelAgentTimeLimitedPaidService, Guid, TravelAgentTimeLimitedPaidServiceDto, TravelAgentTimeLimitedPaidServiceDtoB>
{
    public TravelAgentTimeLimitedPaidServiceController(IGenericRepo<TravelAgentTimeLimitedPaidService, Guid> repo) : base(repo) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TravelAgentTimeLimitedPaidServiceDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create(TravelAgentTimeLimitedPaidServiceDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, TravelAgentTimeLimitedPaidServiceDtoB dtoB)
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
