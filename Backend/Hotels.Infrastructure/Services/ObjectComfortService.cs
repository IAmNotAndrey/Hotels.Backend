using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Services;

public class ObjectComfortService : IObjectComfortService
{
    private readonly ApplicationContext _db;
    private readonly IGenericRepo<ObjectComfort, Guid> _objectComfortGenRepo;

    public ObjectComfortService(ApplicationContext db,
                             IGenericRepo<ObjectComfort, Guid> objectComfortGenRepo)
    {
        _db = db;
        _objectComfortGenRepo = objectComfortGenRepo;
    }

    public async Task LinkAsync(string partnerId, Guid objectComfortId)
    {
        Partner partner = await _db.Partners
            .Include(e => e.Comforts)
            .FirstAsync(e => e.Id == partnerId);
        ObjectComfort comfort = await _objectComfortGenRepo.GetByIdAsync(objectComfortId, asNoTracking: false);
        if (partner.Comforts.Contains(comfort))
        {
            throw new InvalidOperationException($"{nameof(ObjectComfort)} cannot be linked to {nameof(Partner)} because it's already linked.");
        }
        partner.Comforts.Add(comfort);
        await _db.SaveChangesAsync();
    }

    public async Task UnlinkAsync(string partnerId, Guid objectComfortId)
    {
        Partner partner = await _db.Partners
            .Include(e => e.Comforts)
            .FirstAsync(e => e.Id == partnerId);
        ObjectComfort comfort = await _objectComfortGenRepo.GetByIdAsync(objectComfortId, asNoTracking: false);
        if (!partner.Comforts.Contains(comfort))
        {
            throw new InvalidOperationException($"{nameof(ObjectComfort)} cannot be unlinked from {nameof(Partner)} because it's not linked yet.");
        }
        partner.Comforts.Remove(comfort);
        await _db.SaveChangesAsync();
    }
}
