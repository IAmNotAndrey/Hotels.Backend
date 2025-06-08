using AutoMapper;
using Hotels.Application.Exceptions;
using Hotels.Domain.Common.Interfaces;
using Hotels.PartnerReviews.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.PartnerReviews.Persistence.Repositories;

public class GenericRepo<TEntity, TKey> : IGenericRepo<TEntity, TKey>
    where TEntity : class, IKey<TKey>
    where TKey : notnull
{
    private readonly DbContext _db;
    private readonly IMapper _mapper;

    public GenericRepo(DbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public IQueryable<TEntity> Entities => _db.Set<TEntity>();

    #region GetById

    public async Task<TDto?> GetDtoOrDefaultAsync<TDto>(TKey id)
    {
        TEntity? entity = await GetByIdOrDefaultAsync(id, asNoTracking: true);
        // fixme: what if null?
        TDto dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public async Task<TDto> GetDtoAsync<TDto>(TKey id)
    {
        TDto dto = await GetDtoOrDefaultAsync<TDto>(id)
            ?? throw new EntityNotFoundException();
        return dto;
    }

    public async Task<TEntity?> GetByIdOrDefaultAsync(TKey id, bool asNoTracking = true)
    {
        if (asNoTracking)
        {
            return await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
        }
        return await _db.Set<TEntity>().AsTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public async Task<TEntity> GetByIdAsync(TKey id, bool asNoTracking = true)
    {
        TEntity entity = await GetByIdOrDefaultAsync(id, asNoTracking: asNoTracking)
            ?? throw new EntityNotFoundException();
        return entity;
    }

    #endregion

    #region GetAll

    public async Task<TDto[]> GetAllDtosAsync<TDto>()
    {
        TEntity[] entities = await GetAllAsync(asNoTracking: true);
        TDto[] dtos = GetMapped<TDto>(entities);
        return dtos;
    }

    private async Task<TEntity[]> GetAllAsync(bool asNoTracking = true)
    {
        if (asNoTracking)
        {
            return await _db
                .Set<TEntity>()
                .AsNoTracking()
                .ToArrayAsync();
        }
        return await _db
            .Set<TEntity>()
            .ToArrayAsync();
    }

    #endregion

    #region Add

    public async Task<TEntity> AddAsync<TDtoB>(TDtoB dtoB)
    {
        TEntity entity = _mapper.Map<TDtoB, TEntity>(dtoB);
        return await AddAsync(entity);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _db.Set<TEntity>().AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    #endregion

    #region Update

    public async Task UpdateAsync<TDtoB>(TKey id, TDtoB dtoB)
    {
        TEntity entity = await GetByIdOrDefaultAsync(id, asNoTracking: false)
            ?? throw new EntityNotFoundException($"Entity wasn't found in '{nameof(DbContext)}'");
        _mapper.Map(dtoB, entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        TEntity? exist = await _db.Set<TEntity>().FindAsync(entity.Id)
            ?? throw new EntityNotFoundException($"Entity wasn't found in '{nameof(DbContext)}'");
        _db.Entry(exist).CurrentValues.SetValues(entity);
        await _db.SaveChangesAsync();
    }

    #endregion

    #region Delete

    public async Task DeleteAsync(TKey id)
    {
        TEntity entity = await GetByIdOrDefaultAsync(id)
            ?? throw new EntityNotFoundException($"Entity wasn't found in '{nameof(DbContext)}'");
        await DeleteAsync(entity);
    }

    private async Task DeleteAsync(TEntity entity)
    {
        _db.Set<TEntity>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    #endregion

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await GetByIdOrDefaultAsync(id) != null;
    }

    private TDto[] GetMapped<TDto>(IEnumerable<TEntity> entities)
    {
        return entities.Select(e => _mapper.Map<TDto>(e)).ToArray();
    }
}
