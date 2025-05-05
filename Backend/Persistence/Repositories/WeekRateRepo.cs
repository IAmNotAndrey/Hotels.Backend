using AutoMapper;
using Hotels.ClassLibrary.Business.DtoBs;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.WeekRates;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class WeekRateRepo : IWeekRateRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;

    public WeekRateRepo(ApplicationContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<SubobjectWeekRate> CreateAsync(SubobjectWeekRateDtoB dtoB)
    {
        Subobject subobject = await _db.Subobjects
            .Include(e => e.WeekRate) // NOTE: Don't forget in order to delete the previous one.
            .FirstAsync(s => s.Id == dtoB.SubobjectId);
        SubobjectWeekRate weekRate = _mapper.Map<SubobjectWeekRate>(dtoB);
        subobject.WeekRate = weekRate;
        await _db.SaveChangesAsync();
        return subobject.WeekRate;
    }
}
