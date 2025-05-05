using Hotels.ClassLibrary.Business.DtoBs;
using Hotels.Domain.Entities.WeekRates;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IWeekRateRepo
{
    Task<SubobjectWeekRate> CreateAsync(SubobjectWeekRateDtoB dtoB);
}
