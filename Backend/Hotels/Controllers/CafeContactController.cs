namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CafeContactController : ApplicationControllerBase<CafeContact, Guid, CafeContactDto, CafeContactDtoB>
{
    public CafeContactController(IGenericRepo<CafeContact, Guid> repo) : base(repo) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CafeContactDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CafeContactDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] CafeContactDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await base.Delete_(id);
    }
}
