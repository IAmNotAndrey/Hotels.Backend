using Hotels.Bookings.Persistence.Interfaces.Repositories;
using Hotels.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Bookings.Persistence.Repositories;

public class BookingRepo(DbContext db
) : IBookingRepo
{
    public async Task<Booking> GetByPaymentIdAsync(string paymentId)
    {
        return await db.Set<Booking>()
            .AsNoTracking()
            .FirstAsync(b => b.PaymentId == paymentId);
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsBySubobjectIdAsync(Guid sbobjectId)
    {
        return await db.Set<Booking>()
            .AsNoTracking()
            .Where(b => b.SubobjectId == sbobjectId)
            .ToListAsync();
    }
}
