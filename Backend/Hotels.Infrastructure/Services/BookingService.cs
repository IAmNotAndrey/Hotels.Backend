using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Persistence.Interfaces.Repositories;

namespace Hotels.Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IGenericRepo<Booking, Guid> _repo;

    public BookingService(IGenericRepo<Booking, Guid> repo)
    {
        _repo = repo;
    }

    public async Task<bool> HasBookingConflictAsync(Guid bookingId, DateOnly startDate, DateOnly endDate)
    {
        Booking booking = await _repo.GetByIdAsync(bookingId);

        // Convert DateOnly to DateTime for comparison.
        DateTime startTime = startDate.ToDateTime(new TimeOnly(0, 0));
        DateTime endTime = endDate.ToDateTime(new TimeOnly(23, 59));

        // Check whether the new dates overlap with the existing ones.
        bool res = startTime < booking.DateOut.ToDateTime(new TimeOnly(23, 59)) && endTime > booking.DateIn.ToDateTime(new TimeOnly(0, 0));
        return res;
    }
}
