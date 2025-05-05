using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.StaticFiles;

namespace Hotels.Domain.Entities.Reviews;

public abstract class Review : ApplicationNamedEntity, ICreatedAt, IHasImageLinks<ReviewImageLink>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public float Rating { get; set; }

    public string Comment { get; set; } = null!;

    public ICollection<ReviewImageLink> ImageLinks { get; set; } = [];

    public override string ToString()
    {
        return $"{nameof(Review)}_{Id}";
    }
}
