namespace Hotels.Controllers.EntityTypes;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class TourTypeController : ApplicationControllerBase<TourType, Guid, TourTypeDto, TourTypeDtoB>
{
    public TourTypeController(IGenericRepo<TourType, Guid> repo) : base(repo) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourTypeDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TourTypeDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] TourTypeDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, TourTypeDtoB dtoB)
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
