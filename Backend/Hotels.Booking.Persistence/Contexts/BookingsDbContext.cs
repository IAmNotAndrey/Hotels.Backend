using Hotels.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Bookings.Persistence.Contexts;

public class BookingsDbContext : DbContext
{
    public virtual DbSet<Booking> Bookings { get; init; }

    public BookingsDbContext(DbContextOptions<BookingsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
