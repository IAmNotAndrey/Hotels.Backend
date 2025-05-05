using Hotels.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Domain.Common;

public abstract class ApplicationBaseEntity : IKey<Guid>
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();

    public override bool Equals(object? obj)
    {
        if (obj is ApplicationBaseEntity other)
        {
            return Id.Equals(other.Id);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
