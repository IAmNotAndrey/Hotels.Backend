namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class AttractionController : ApplicationControllerBase<Attraction, Guid, AttractionDto, AttractionDtoB>
{
    private readonly IAttractionRepo _attractionRepo;

    public AttractionController(IGenericRepo<Attraction, Guid> repo, IAttractionRepo attractionRepo) : base(repo)
    {
        _attractionRepo = attractionRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttractionDto>>> Get()
    {
        return Ok(await _attractionRepo.GetDtosIncludedAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AttractionDto>> Get(Guid id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{nameof(Attraction)} wasn't found.");
        }
        return Ok(await _attractionRepo.GetDtoIncludedAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create(AttractionDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, AttractionDtoB dtoB)
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
