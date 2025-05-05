using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Feeds;

public class ObjectFeed : Feed
{
    public ICollection<Partner> Partners { get; set; } = [];
}
