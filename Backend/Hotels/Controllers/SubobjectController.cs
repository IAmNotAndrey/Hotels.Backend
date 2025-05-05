namespace Hotels.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class SubobjectController : ControllerBase
{
    private readonly IHousingRepo _subobjectRepo;

    public SubobjectController(IHousingRepo subobjectRepo)
    {
        _subobjectRepo = subobjectRepo;
    }

    [HttpGet]
    public ActionResult<ImmutableList<string>> GetSubobjectChildrenTypeNames()
    {
        var typeNames = _subobjectRepo.SubobjectChildrenTypeNames;
        return Ok(typeNames);
    }
}
