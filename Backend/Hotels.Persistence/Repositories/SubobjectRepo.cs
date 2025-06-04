using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class SubobjectRepo(ApplicationContext db) : ISubobjectRepo
{
    public async Task<Subobject> GetSubobjectWithBookingsAsync(Guid subobjectId)
    {
        return await db.Subobjects
            .Include(s => s.Bookings)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == subobjectId)
            ?? throw new EntityNotFoundException($"{nameof(Subobject)} with Id '{subobjectId}' not found");
    }
}
