using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Services;

public class BathroomService : IBathroomService
{
    private readonly ApplicationContext _db;

    public BathroomService(ApplicationContext db)
    {
        _db = db;
    }
    public async Task LinkAsync(Guid subobjectId, Guid bathroomId)
    {
        Bathroom bathroom = await _db.Bathrooms.FirstAsync(e => e.Id == bathroomId);
        Subobject subobject = await _db.Subobjects
            .Include(e => e.Bathrooms)
            .FirstAsync(e => e.Id == subobjectId);
        if (subobject.Bathrooms.Contains(bathroom))
        {
            throw new InvalidOperationException($"{nameof(Bathroom)} cannot be linked to {nameof(Subobject)} because it's already linked.");

        }
        subobject.Bathrooms.Add(bathroom);
        await _db.SaveChangesAsync();
    }

    public async Task UnlinkAsync(Guid subobjectId, Guid bathroomId)
    {
        Bathroom bathroom = await _db.Bathrooms.FirstAsync(e => e.Id == bathroomId);
        Subobject subobject = await _db.Subobjects
            .Include(e => e.Bathrooms)
            .FirstAsync(e => e.Id == subobjectId);
        if (!subobject.Bathrooms.Contains(bathroom))
        {
            throw new InvalidOperationException($"{nameof(Bathroom)} cannot be unlinked from {nameof(Subobject)} because it's not linked yet.");

        }
        subobject.Bathrooms.Remove(bathroom);
        await _db.SaveChangesAsync();
    }
}
