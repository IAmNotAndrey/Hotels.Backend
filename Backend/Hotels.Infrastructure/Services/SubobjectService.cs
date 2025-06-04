using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Services;

public class SubobjectService : ISubobjectService
{
    private readonly ApplicationContext _db;

    public SubobjectService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<decimal> CalculateBookingCostAsync(Guid subobjectId, DateOnly dateIn, DateOnly dateOut)
    {
        var subobject = await _db.Subobjects
            .Include(s => s.WeekRate)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == subobjectId)
            ?? throw new EntityNotFoundException($"{nameof(Subobject)} with Id {subobjectId} wasn't found");
        // Вызываем исключение, если не заданы цены на неделю
        if (subobject.WeekRate == null)
        {
            throw new EntityNotFoundException($"WeekRate for Subobject with Id {subobjectId} wasn't found.");
        }

        DateTime startDate = dateIn.ToDateTime(new TimeOnly(0, 0));
        DateTime endDate = dateOut.ToDateTime(new TimeOnly(23, 59));

        decimal totalCost = 0;
        for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
        {
            decimal? rateForDay = subobject.WeekRate[currentDate.DayOfWeek];

            if (!rateForDay.HasValue)
            {
                throw new InvalidOperationException($"No week rate for '{currentDate.DayOfWeek}'.");
            }
            totalCost += rateForDay.Value;
        }

        return totalCost;
    }
}
