namespace Hotels.Controllers;

[ApiController]
public abstract class ApplicationControllerBase<TEntity, TKey, TDto, TDtoB> : ControllerBase
    where TEntity : class, IKey<TKey>
    where TKey : notnull
{
    protected readonly IGenericRepo<TEntity, TKey> _repo;

    protected ApplicationControllerBase(IGenericRepo<TEntity, TKey> repo)
    {
        _repo = repo;
    }

    protected async Task<ActionResult<IEnumerable<TDto>>> Get_()
    {
        TDto[] dtos = await _repo.GetAllDtosAsync<TDto>();
        return Ok(dtos);
    }

    protected async Task<ActionResult<TDto>> Get_(TKey id)
    {
        TDto? dto = await _repo.GetDtoOrDefaultAsync<TDto>(id);
        return dto != null ? Ok(dto) : NotFound($"{typeof(TEntity).Name} wasn't found.");
    }

    protected async Task<ActionResult<TKey>> Create_([FromForm] TDtoB dtoB, string actionName)
    {
        TEntity entity = await _repo.AddAsync(dtoB);
        return CreatedAtAction(actionName, entity.Id);
    }

    protected async Task<IActionResult> Update_(TKey id, TDtoB dtoB)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{typeof(TEntity).Name} wasn't found.");
        }
        await _repo.UpdateAsync(id, dtoB);
        return Ok();
    }

    protected async Task<IActionResult> Delete_(TKey id)
    {
        if (!await _repo.ExistsAsync(id))
        {
            return NotFound($"{typeof(TEntity).Name} wasn't found.");
        }
        await _repo.DeleteAsync(id);
        return Ok();
    }
}