using Hotels.Domain.Entities.WeekRates;
using Hotels.Presentation.DtoBs;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IWeekRateRepo
{
    Task<SubobjectWeekRate> CreateAsync(SubobjectWeekRateDtoB dtoB);
}
