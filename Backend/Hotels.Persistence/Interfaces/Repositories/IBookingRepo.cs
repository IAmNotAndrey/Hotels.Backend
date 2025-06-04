using Hotels.Domain.Entities;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IBookingRepo
{
    Task<Booking> GetByPaymentIdAsync(string paymentId);
}
