using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities.Feeds;

public class SubobjectFeed : Feed
{
    public ICollection<Subobject> Subobjects { get; set; } = [];
}
