using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class SubobjectRepo : ISubobjectRepo
{
    private readonly ApplicationContext _db;
    private readonly IBookingRepo _bookingRepo;

    public SubobjectRepo(ApplicationContext db, IBookingRepo bookingRepo)
    {
        _db = db;
        _bookingRepo = bookingRepo;
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

    public async Task<bool> HasBookingConflictAsync(Guid subobjectId, DateOnly startDate, DateOnly endDate)
    {
        Subobject subobject = await _db.Subobjects
            .Include(s => s.Bookings)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == subobjectId)
            ?? throw new EntityNotFoundException($"{nameof(Subobject)} with Id {subobjectId} wasn't found");

        // Проверяем, пересекаются ли новые даты с уже существующими
        return subobject.Bookings.Any(b => _bookingRepo.HasBookingConflictAsync(b.Id, startDate, endDate).Result);
    }
}
