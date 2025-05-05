namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class PromotionController : ControllerBase
{
    private const string CountrySubjectNotFoundText = $"{nameof(CountrySubject)} wasn't found.";

    private readonly IPartnerRepo _partnerRepo;
    private readonly IGenericRepo<CountrySubject, Guid> _countrySubjectGenRepo;

    public PromotionController(IPartnerRepo partnerRepo,
                               IGenericRepo<CountrySubject, Guid> countrySubjectGenRepo)
    {
        _partnerRepo = partnerRepo;
        _countrySubjectGenRepo = countrySubjectGenRepo;
    }

    /// <summary>
    /// Get all publishing ('IsPublished') 'Partner's in the selected country subject.
    /// </summary>
    [HttpGet("{countrySubjectId}")]
    public async Task<ActionResult<IEnumerable<PartnerDto>>> GetAdvertisingObjects(Guid countrySubjectId)
    {
        if (!await _countrySubjectGenRepo.ExistsAsync(countrySubjectId))
        {
            return NotFound(CountrySubjectNotFoundText);
        }
        IEnumerable<PartnerDto> dtos = await _partnerRepo.GetDtosIncludedAdvertisingAsync(countrySubjectId);
        return Ok(dtos);
    }

    /// <summary>
    /// Get all publishing ('IsPromoSeries') 'Partner's in the selected country subject.
    /// </summary>
    [HttpGet("{countrySubjectId}")]
    public async Task<ActionResult<IEnumerable<PartnerDto>>> GetPromoSeriesObjects(Guid countrySubjectId)
    {
        if (!await _countrySubjectGenRepo.ExistsAsync(countrySubjectId))
        {
            return NotFound(CountrySubjectNotFoundText);
        }
        IEnumerable<PartnerDto> dtos = await _partnerRepo.GetDtosIncludedPromoSeriesAsync(countrySubjectId);
        return Ok(dtos);
    }
}
