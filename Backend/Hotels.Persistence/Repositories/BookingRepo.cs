using Hotels.Domain.Entities;
using Hotels.Persistence.Interfaces.Repositories;

namespace Hotels.Persistence.Repositories;

public class BookingRepo : IBookingRepo
{
    private readonly IGenericRepo<Booking, Guid> _repo;

    public BookingRepo(IGenericRepo<Booking, Guid> repo)
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
