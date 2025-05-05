using Hotels.Application.Dtos;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IAttractionRepo
{
    /// <summary>
    /// Retrieves all attractions from the database, including related data such as image links, 
    /// and maps them to an array of DTOs.
    /// </summary>
    /// <returns>An array of <see cref="AttractionDto"/> containing all attractions with related data.</returns>
    Task<IEnumerable<AttractionDto>> GetDtosIncludedAsync();

    /// <summary>
    /// Retrieves a specific attraction by its unique identifier, including related data such as image links, 
    /// and maps it to a DTO.
    /// </summary>
    /// <param name="id">The unique identifier of the attraction.</param>
    /// <returns>
    /// A <see cref="AttractionDto"/> representing the attraction with related data.
    /// Throws <see cref="InvalidOperationException"/> if the attraction is not found.
    /// </returns>
    Task<AttractionDto> GetDtoIncludedAsync(Guid id);
}
