using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class SubobjectComfortRepo : ISubobjectComfortRepo
{
    private readonly ApplicationContext _db;
    private readonly IGenericRepo<SubobjectComfort, Guid> _subobjectComfortGenRepo;

    public SubobjectComfortRepo(ApplicationContext db,
                                IGenericRepo<SubobjectComfort, Guid> subobjectComfortGenRepo)
    {
        _db = db;
        _subobjectComfortGenRepo = subobjectComfortGenRepo;
    }

    public async Task LinkAsync(Guid subobjectId, Guid subobjectComfortId)
    {
        Subobject subobject = await _db.Subobjects
           .Include(e => e.Comforts)
           .FirstAsync(e => e.Id == subobjectId);
        SubobjectComfort comfort = await _subobjectComfortGenRepo.GetByIdAsync(subobjectComfortId, asNoTracking: false);
        if (subobject.Comforts.Contains(comfort))
        {
            throw new InvalidOperationException($"{nameof(SubobjectComfort)} cannot be linked to {nameof(Subobject)} because it's already linked.");
        }
        subobject.Comforts.Add(comfort);
        await _db.SaveChangesAsync();
    }

    public async Task UnlinkAsync(Guid subobjectId, Guid subobjectComfortId)
    {
        Subobject subobject = await _db.Subobjects
            .Include(e => e.Comforts)
            .FirstAsync(e => e.Id == subobjectId);
        SubobjectComfort comfort = await _subobjectComfortGenRepo.GetByIdAsync(subobjectComfortId, asNoTracking: false);
        if (!subobject.Comforts.Contains(comfort))
        {
            throw new InvalidOperationException($"{nameof(SubobjectComfort)} cannot be unlinked from {nameof(Subobject)} because it's not linked yet.");
        }
        subobject.Comforts.Remove(comfort);
        await _db.SaveChangesAsync();
    }
}
