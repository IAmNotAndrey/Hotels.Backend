namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class ImageController : ControllerBase
{
    private const string LogoSubDir = "Logos";

    private readonly ApplicationContext _db;
    private readonly IImageStorageRepo _imageStorageRepo;
    private readonly IApplicationUserRepo _appUserRepo;
    private readonly IImageRepo _imageRepo;

    public ImageController(ApplicationContext db, IImageStorageRepo imageStorageRepo, IApplicationUserRepo appUserRepo, IImageRepo imageRepo)
    {
        _db = db;
        _imageStorageRepo = imageStorageRepo;
        _appUserRepo = appUserRepo;
        _imageRepo = imageRepo;
    }

    /// <summary>
    /// Устанавливает <see cref="ApplicationObject.ImageLinks"/>
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> SetToObject([Required, FromForm] string objectId, [FromForm, MaxLength(10)] List<IFormFile> files, [FromForm, MaxLength(10)] List<bool> areTitle)
    {
        ApplicationObject? appObject = await _db.ApplicationObjects
            .Include(e => e.ImageLinks)
            .FirstOrDefaultAsync(e => e.Id == objectId);
        if (appObject == null)
        {
            return NotFound($"{nameof(ApplicationObject)} wasn't found");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, objectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        List<TitledImageDtoB> dtos = _imageRepo.CreateTitledImageDtos(files, areTitle).ToList();

        await _imageStorageRepo.SaveAndBindImagesAsync(
            appObject,
            dtos,
            uri => new ObjectImageLink { ApplicationObjectId = objectId, Uri = uri }
        );

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> SetLogoToTravelAgent([Required] string travelAgentId, IFormFile logo)
    {
        TravelAgent? travelAgent = await _db.TravelAgents
            .Include(e => e.LogoLink)
            .FirstOrDefaultAsync(e => e.Id == travelAgentId);
        if (travelAgent == null)
        {
            return NotFound();
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, travelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        // Сохраняем лого по пути: `../Logos/travelAgentId/logo.png`
        string uri = await _imageStorageRepo.SaveFileAsync(logo, Path.Combine(LogoSubDir, travelAgent.ToString()));

        // Происходит добавление нового лого. Если до этого уже было установлено другое лого, оно будет удалено
        await _db.TravelAgentLogoLinks.AddAsync(new() { TravelAgentId = travelAgentId, Uri = uri });
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Tourist)},{nameof(Admin)}")]
    public async Task<IActionResult> SetToReview([Required] Guid reviewId, [FromForm, MaxLength(10)] List<IFormFile> files, [FromForm, MaxLength(10)] List<bool> areTitle)
    {
        PartnerReview? review = await _db.ObjectReviews
            .Include(e => e.ImageLinks)
            .FirstOrDefaultAsync(e => e.Id == reviewId);
        if (review == null)
        {
            return NotFound($"{nameof(PartnerReview)} wasn't found");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, review.TouristId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var dtos = _imageRepo.CreateTitledImageDtos(files, areTitle);

        await _imageStorageRepo.SaveAndBindImagesAsync(
            review,
            dtos,
            uri => new ReviewImageLink { ReviewId = reviewId, Uri = uri }
        );

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(Partner)},{nameof(Admin)}")]
    public async Task<IActionResult> SetToSubobject([Required] Guid subobjectId, [FromForm, MaxLength(10)] List<IFormFile> files, [FromForm, MaxLength(10)] List<bool> areTitle)
    {
        Subobject? subobject = await _db.Subobjects
            .Include(e => e.ImageLinks)
            .FirstOrDefaultAsync(e => e.Id == subobjectId);
        if (subobject == null)
        {
            return NotFound($"{nameof(Subobject)} wasn't found");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, subobject.PartnerId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var dtos = _imageRepo.CreateTitledImageDtos(files, areTitle);

        await _imageStorageRepo.SaveAndBindImagesAsync(
            subobject,
            dtos,
            uri => new SubobjectImageLink { SubobjectId = subobjectId, Uri = uri }
        );

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(TravelAgent)},{nameof(Admin)}")]
    public async Task<IActionResult> SetToTour([Required] Guid tourId, [FromForm, MaxLength(10)] List<IFormFile> files, [FromForm, MaxLength(10)] List<bool> areTitle)
    {
        Tour? tour = await _db.Tours
            .Include(e => e.ImageLinks)
            .FirstOrDefaultAsync(e => e.Id == tourId);
        if (tour == null)
        {
            return NotFound($"{nameof(Tour)} wasn't found");
        }
        // Does the requester do an allowed operation?
        if (!await _appUserRepo.IsUserAllowedAsync(User, tour.TravelAgentId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var dtos = _imageRepo.CreateTitledImageDtos(files, areTitle);

        await _imageStorageRepo.SaveAndBindImagesAsync(
            tour,
            dtos,
            uri => new TourImageLink { TourId = tourId, Uri = uri }
        );

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> SetToAttraction([Required] Guid attractionId, [FromForm, MaxLength(10)] List<IFormFile> files, [FromForm, MaxLength(10)] List<bool> areTitle)
    {
        Attraction? attraction = await _db.Attractions
            .Include(e => e.ImageLinks)
            .FirstOrDefaultAsync(e => e.Id == attractionId);
        if (attraction == null)
        {
            return NotFound($"{nameof(Attraction)} wasn't found");
        }

        var dtos = _imageRepo.CreateTitledImageDtos(files, areTitle);

        await _imageStorageRepo.SaveAndBindImagesAsync(
            attraction,
            dtos,
            uri => new AttractionImageLink { AttractionId = attractionId, Uri = uri }
        );

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> SetToCafe([Required] Guid cafeId, [FromForm, MaxLength(10)] List<IFormFile> files, [FromForm, MaxLength(10)] List<bool> areTitle)
    {
        Cafe? cafe = await _db.Cafes
            .Include(e => e.ImageLinks)
            .FirstOrDefaultAsync(e => e.Id == cafeId);
        if (cafe == null)
        {
            return NotFound($"{nameof(Cafe)} wasn't found");
        }

        var dtos = _imageRepo.CreateTitledImageDtos(files, areTitle);

        await _imageStorageRepo.SaveAndBindImagesAsync(
            cafe,
            dtos,
            uri => new CafeImageLink { CafeId = cafeId, Uri = uri }
        );

        await _db.SaveChangesAsync();

        return Ok();
    }
}
