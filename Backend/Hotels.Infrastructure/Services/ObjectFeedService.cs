using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Feeds;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Services;

public class ObjectFeedService : IObjectFeedService
{
    private readonly ApplicationContext _db;

    public ObjectFeedService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task LinkAsync(string partnerId, Guid feedId)
    {
        Partner partner = await _db.Partners
            .Include(e => e.Feeds)
            .FirstAsync(e => e.Id == partnerId);
        ObjectFeed objectFeed = await _db.ObjectFeeds.FirstAsync(e => e.Id == feedId);
        if (partner.Feeds.Contains(objectFeed))
        {
            throw new InvalidOperationException($"{nameof(ObjectFeed)} cannot be linked to {nameof(Partner)} because it's already linked.");
        }
        partner.Feeds.Add(objectFeed);
        await _db.SaveChangesAsync();
    }

    public async Task UnlinkAsync(string partnerId, Guid feedId)
    {
        Partner partner = await _db.Partners
            .Include(e => e.Feeds)
            .FirstAsync(e => e.Id == partnerId);
        ObjectFeed objectFeed = await _db.ObjectFeeds.FirstAsync(e => e.Id == feedId);
        if (!partner.Feeds.Contains(objectFeed))
        {
            throw new InvalidOperationException($"{nameof(ObjectFeed)} cannot be unlinked from {nameof(Partner)} because it's not linked yet.");
        }
        partner.Feeds.Remove(objectFeed);
        await _db.SaveChangesAsync();
    }
}
