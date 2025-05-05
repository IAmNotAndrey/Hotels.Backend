using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class ToiletRepo : IToiletRepo
{
    private readonly ApplicationContext _db;

    public ToiletRepo(ApplicationContext db)
    {
        _db = db;
    }

    public async Task LinkAsync(Guid subobjectId, Guid toiletId)
    {
        Subobject subobject = await _db.Subobjects.FirstAsync(e => e.Id == subobjectId);
        Toilet toilet = await _db.Toilets.FirstAsync(e => e.Id == toiletId);
        subobject.Toilets.Add(toilet);
        await _db.SaveChangesAsync();
    }

    public async Task UnlinkAsync(Guid subobjectId, Guid toiletId)
    {
        Subobject subobject = await _db.Subobjects
            .Include(e => e.Toilets)
            .FirstAsync(e => e.Id == subobjectId);
        Toilet toilet = await _db.Toilets.FirstAsync(e => e.Id == toiletId);
        if (!subobject.Toilets.Contains(toilet))
        {
            throw new InvalidOperationException($"{nameof(Toilet)} wasn't found in {nameof(Subobject.Toilets)}.");
        }
        subobject.Toilets.Remove(toilet);
        await _db.SaveChangesAsync();
    }
}
