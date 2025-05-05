namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CafeController : ApplicationControllerBase<Cafe, Guid, CafeDto, CafeDtoB>
{
    public CafeController(IGenericRepo<Cafe, Guid> repo) : base(repo) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CafeDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CafeDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<CafeDto>>> GetByFilter(CafeFilter filter)
    {
        return Ok(await _repo.GetAllDtosAsync<CafeDto>(filter));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create(CafeDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, CafeDtoB dtoB)
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
