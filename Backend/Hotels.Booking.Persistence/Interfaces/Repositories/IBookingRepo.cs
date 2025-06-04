using Hotels.Domain.Entities;

namespace Hotels.Bookings.Persistence.Interfaces.Repositories;

public interface IBookingRepo
{
    Task<Booking> GetByPaymentIdAsync(string paymentId);
    Task<IReadOnlyList<Booking>> GetBookingsBySubobjectIdAsync(Guid sbobjectId);
}
