using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Services;

public class ToiletService : IToiletService
{
    private readonly ApplicationContext _db;

    public ToiletService(ApplicationContext db)
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
