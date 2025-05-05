namespace Hotels.Controllers.Regions;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CityController : ApplicationControllerBase<City, Guid, CityDto, CityDtoB>
{
    private readonly ICityRepo _cityRepo;

    public CityController(IGenericRepo<City, Guid> repo, ICityRepo cityRepo) : base(repo)
    {
        _cityRepo = cityRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CityDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpGet("{countrySubjectId}")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetByCountrySubjectId(Guid countrySubjectId)
    {
        return Ok(await _cityRepo.GetDtosByCountrySubjectAsync(countrySubjectId));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create([FromForm] CityDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, CityDtoB dtoB)
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
