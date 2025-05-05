using Hotels.Application.Exceptions;
using Hotels.ClassLibrary.Interfaces;
using Hotels.Domain.Common.Interfaces;

namespace Hotels.Persistence.Interfaces.Repositories;

/// <summary>
/// Generic repository interface for managing entities in the database.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IGenericRepo<TEntity, TKey>
    where TEntity : class, IKey<TKey>
    where TKey : notnull
{
    /// <summary>
    /// Gets the queryable collection of all entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    IQueryable<TEntity> Entities { get; }

    /// <summary>
    /// Gets an entity by its unique <paramref name="id"/> and optionally tracks it in the context.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="asNoTracking">
    /// Specifies whether the entity should be got with tracking disabled. 
    /// Defaults to <see langword="true"/>.
    /// </param>
    /// <returns>
    /// The entity if it's found, <see langword="null"/> otherwise.
    /// </returns>
    Task<TEntity?> GetByIdOrDefaultAsync(TKey id, bool asNoTracking = true);

    /// <summary>
    /// Gets an entity by its unique <paramref name="id"/> and optionally tracks it in the context.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="asNoTracking">
    /// Specifies whether the entity should be got with tracking disabled. 
    /// Defaults to <see langword="true"/>.
    /// </param>
	/// <exception cref="EntityNotFoundException"></exception>
    /// <returns>
    /// The entity if it's found.
    /// </returns>
    Task<TEntity> GetByIdAsync(TKey id, bool asNoTracking = true);

    /// <summary>
    /// Gets an entity by its <paramref name="id"/> and maps it to a DTO of type <typeparamref name="TDto"/>.
    /// </summary>
    /// <typeparam name="TDto">The type to which the entity will be mapped.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    /// Mapped DTO if the entity is found, <see langword="null"/> otherwise.
    /// </returns>
    Task<TDto?> GetDtoOrDefaultAsync<TDto>(TKey id);

    Task<TDto> GetDtoAsync<TDto>(TKey id);

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TEntity"/> and maps them to DTOs of type <typeparamref name="TDto"/>.
    /// </summary>
    /// <typeparam name="TDto">The type to which the entities will be mapped.</typeparam>
    /// <returns>
    /// Array of mapped DTOs.
    /// </returns>
    Task<TDto[]> GetAllDtosAsync<TDto>();

    /// <summary>
    /// Retrieves all entities from the database that match the specified filter criteria, 
    /// and maps them to an array of DTOs of type <typeparamref name="TDto"/>.
    /// </summary>
    /// <typeparam name="TDto">The type to which the entities will be mapped.</typeparam>
    /// <param name="filter">An object implementing <see cref="IFilterModel{TEntity}"/> that defines the filtering criteria.</param>
    /// <returns>An array of <typeparamref name="TDto"/> representing the filtered entities.</returns>
    Task<TDto[]> GetAllDtosAsync<TDto>(IFilterModel<TEntity> filter);

    Task<TEntity[]> GetAllAsync(IFilterModel<TEntity> filter, bool asNoTracking = true);

    /// <summary>
    /// Asynchronously adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>
    /// The added entity.
    /// </returns>
    /// <remarks>
    /// This method saves changes to the database immediately after adding the entity.
    /// </remarks>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Maps the provided DTO of type <typeparamref name="TDtoB"/> to an entity of type <typeparamref name="TEntity"/> 
    /// and adds it to the database.
    /// </summary>
    /// <typeparam name="TDtoB">The type of the input DTO.</typeparam>
    /// <returns>
    /// Added entity.
    /// </returns>
    Task<TEntity> AddAsync<TDtoB>(TDtoB dtoB);

    /// <summary>
    /// Maps the provided DTO of type <typeparamref name="TDtoB"/> to an entity and updates an existing entity
    /// identified by <paramref name="id"/> in the database.
    /// </summary>
    /// <typeparam name="TDtoB">The type of the input DTO.</typeparam>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <param name="dtoB">The DTO containing updated data.</param>
    Task UpdateAsync<TDtoB>(TKey id, TDtoB dtoB);

    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity from the database identified by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    Task DeleteAsync(TKey id);

    /// <summary>
    /// Checks whether an entity with the specified <paramref name="id"/> exists in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to check.</param>
    /// <returns>
    /// <see langword="true"/> if the entity exists;
    /// <see langword="false"/> otherwise.
    /// </returns>
    Task<bool> ExistsAsync(TKey id);
}
