namespace Hotels.Domain.Common;

public abstract class ApplicationNamedEntity : ApplicationBaseEntity
{
    public virtual string Name { get; set; } = null!;
}
