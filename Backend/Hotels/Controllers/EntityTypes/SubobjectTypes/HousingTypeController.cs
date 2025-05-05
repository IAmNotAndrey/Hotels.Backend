namespace Hotels.Controllers.EntityTypes.SubobjectTypes;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class HousingTypeController : ApplicationControllerBase<HousingType, Guid, HousingTypeDto, HousingTypeDtoB>
{
    public HousingTypeController(IGenericRepo<HousingType, Guid> repo) : base(repo) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HousingTypeDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HousingTypeDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] HousingTypeDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, HousingTypeDtoB dtoB)
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
