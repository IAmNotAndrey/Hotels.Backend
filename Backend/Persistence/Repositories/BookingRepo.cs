using Hotels.Application.Exceptions;
using Hotels.Domain.Entities;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class BookingRepo : IBookingRepo
{
    public readonly ApplicationContext _db;

    public BookingRepo(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<bool> HasBookingConflictAsync(Guid bookingId, DateOnly startDate, DateOnly endDate)
    {
        Booking booking = await _db.Bookings.FirstOrDefaultAsync(e => e.Id == bookingId)
            ?? throw new EntityNotFoundException($"{nameof(Booking)} with Id '{bookingId}' wasn't found");

        // Преобразование DateOnly в DateTime для сравнения
        DateTime start = startDate.ToDateTime(new TimeOnly(0, 0));
        DateTime end = endDate.ToDateTime(new TimeOnly(23, 59));

        // Проверяем, пересекаются ли новые даты с уже существующими
        bool res = start < booking.DateOut.ToDateTime(new TimeOnly(23, 59)) && end > booking.DateIn.ToDateTime(new TimeOnly(0, 0));
        return res;
    }
}
