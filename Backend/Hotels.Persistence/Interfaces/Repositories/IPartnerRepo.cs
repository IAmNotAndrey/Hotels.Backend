using Hotels.Application.Dtos.Users;
using Hotels.Domain.Entities.Users;
using Hotels.Presentation.DtoBs.Users;
using Hotels.Presentation.Interfaces;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IPartnerRepo
{
    Task<Partner> GetIncludedAsync(string id);
    Task<PartnerDto> GetDtoIncludedAsync(string id);
    Task<IEnumerable<PartnerDto>> GetAllDtosIncludedIncludeAsync();
    Task<IEnumerable<PartnerDto>> GetDtosIncludedByFilterAsync(IFilterModel<Partner> filter);
    Task<IEnumerable<PartnerDto>> GetDtosIncludedAdvertisingAsync(Guid countrySubjectId);
    Task<IEnumerable<PartnerDto>> GetDtosIncludedPromoSeriesAsync(Guid countrySubjectId);

    Task UpdateAsync(string id, PartnerDtoB dtoB);
}
