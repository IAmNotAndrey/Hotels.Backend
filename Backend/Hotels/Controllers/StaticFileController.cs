namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class StaticFileController : ControllerBase
{
    private readonly ICafeService _cafeRepo;
    private readonly IGenericRepo<CafeMenuFileLink, Guid> _cafeMenuFileLinkRepo;
    private readonly INearbyRepo _nearbyRepo;
    private readonly IGenericRepo<Nearby, Guid> _genNearbyRepo;
    private readonly IGenericRepo<Cafe, Guid> _genCafeRepo;
    private readonly IGenericRepo<NearbyImageLink, Guid> _nearbyImageLinkRepo;
    private readonly INearbyService _nearbyService;

    public StaticFileController(ICafeService cafeRepo,
                                IGenericRepo<CafeMenuFileLink, Guid> cafeMenuFileLinkRepo,
                                INearbyRepo nearbyRepo,
                                IGenericRepo<Nearby, Guid> genNearbyRepo,
                                IGenericRepo<Cafe, Guid> genCafeRepo,
                                IGenericRepo<NearbyImageLink, Guid> nearbyImageLinkRepo,
                                INearbyService nearbyService)
    {
        _cafeRepo = cafeRepo;
        _cafeMenuFileLinkRepo = cafeMenuFileLinkRepo;
        _nearbyRepo = nearbyRepo;
        _genNearbyRepo = genNearbyRepo;
        _genCafeRepo = genCafeRepo;
        _nearbyImageLinkRepo = nearbyImageLinkRepo;
        _nearbyService = nearbyService;
    }

    [HttpPost("{cafeId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> SetCafeMenu(Guid cafeId, [Required] IFormFile menuFile)
    {
        if (!await _genCafeRepo.ExistsAsync(cafeId))
        {
            return NotFound($"{nameof(Cafe)} wasn't found by id '{cafeId}'");
        }
        await _cafeRepo.SaveMenuFileAsync(cafeId, menuFile);
        return Ok();
    }

    [HttpDelete("{menuId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> DeleteCafeMenu(Guid menuId)
    {
        if (!await _cafeMenuFileLinkRepo.ExistsAsync(menuId))
        {
            return NotFound($"{nameof(CafeMenuFileLink)} wasn't found by id '{menuId}'");
        }
        await _cafeMenuFileLinkRepo.DeleteAsync(menuId);
        return Ok();
    }

    [HttpPost("{nearbyId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> SetNearbyImage(Guid nearbyId, [Required] IFormFile image)
    {
        if (!await _genNearbyRepo.ExistsAsync(nearbyId))
        {
            return NotFound($"{nameof(Nearby)} wasn't found by id '{nearbyId}'");
        }
        await _nearbyService.SetImageLinkAsync(nearbyId, image);
        return Ok();
    }

    [HttpDelete("{imageId:guid}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> DeleteNearbyImage(Guid imageId)
    {
        if (!await _nearbyImageLinkRepo.ExistsAsync(imageId))
        {
            return NotFound($"{nameof(NearbyImageLink)} wasn't found by id '{imageId}'");
        }
        await _nearbyImageLinkRepo.DeleteAsync(imageId);
        return Ok();
    }
}
