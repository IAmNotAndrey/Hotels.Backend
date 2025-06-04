using Hotels.Bookings.Persistence.Interfaces.Repositories;
using Hotels.Domain.Entities;

namespace Hotels.Bookings.Infrastructure.Services;

public class BookingService(IGenericRepo<Booking, Guid> repo, IBookingRepo bookingRepo)
{
    public async Task<bool> HasBookingConflictWithBookingAsync(Guid bookingId, DateOnly startDate, DateOnly endDate)
    {
        Booking booking = await repo.GetByIdAsync(bookingId);

        // Convert DateOnly to DateTime for comparison.
        DateTime startTime = startDate.ToDateTime(new TimeOnly(0, 0));
        DateTime endTime = endDate.ToDateTime(new TimeOnly(23, 59));

        // Check whether the new dates overlap with the existing ones.
        bool res = startTime < booking.DateOut.ToDateTime(new TimeOnly(23, 59)) && endTime > booking.DateIn.ToDateTime(new TimeOnly(0, 0));
        return res;
    }

    public async Task<bool> HasBookingConflictWithSubobjectAsync(Guid subobjectId, DateOnly startDate, DateOnly endDate)
    {
        var bookings = await bookingRepo.GetBookingsBySubobjectIdAsync(subobjectId);

        // Проверяем, пересекаются ли новые даты с уже существующими
        return bookings.Any(b => HasBookingConflictWithBookingAsync(b.Id, startDate, endDate).Result);
    }
}
