using Hotels.Application.Dtos.Subobjects;
using System.Collections.Immutable;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IHousingRepo
{
    ImmutableHashSet<string> SubobjectChildrenTypeNames { get; }

    Task<IEnumerable<HousingDto>> GetDtosIncludedAsync();
    Task<HousingDto> GetDtoIncludedAsync(Guid id);
    Task<IEnumerable<HousingDto>> GetDtosIncludedByPartnerAsync(string partnerId);
}
