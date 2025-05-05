namespace Hotels.Controllers.Regions;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CountrySubjectController : ApplicationControllerBase<CountrySubject, Guid, CountrySubjectDto, CountrySubjectDtoB>
{
    private readonly ICountrySubjectRepo _countrySubjectRepo;
    private readonly IGenericRepo<City, Guid> _cityRepo;

    public CountrySubjectController(ICountrySubjectRepo countrySubjectRepo,
                                    IGenericRepo<City, Guid> cityRepo,
                                    IGenericRepo<CountrySubject, Guid> repo) : base(repo)
    {
        _countrySubjectRepo = countrySubjectRepo;
        _cityRepo = cityRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountrySubjectDto>>> Get()
    {
        return await base.Get_();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CountrySubjectDto>> Get(Guid id)
    {
        return await base.Get_(id);
    }

    [HttpGet("{cityId:guid}")]
    public async Task<ActionResult<CountrySubjectDto>> GetByCity(Guid cityId)
    {
        if (!await _cityRepo.ExistsAsync(cityId))
        {
            return NotFound($"{nameof(City)} wasn't found.");
        }
        return Ok(await _countrySubjectRepo.GetByCityIdAsync(cityId));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<ActionResult<Guid>> Create(CountrySubjectDtoB dtoB)
    {
        return await base.Create_(dtoB, nameof(Create));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Update(Guid id, CountrySubjectDtoB dtoB)
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
