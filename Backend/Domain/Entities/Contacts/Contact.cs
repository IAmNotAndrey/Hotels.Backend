using Hotels.Domain.Common;

namespace Hotels.Domain.Entities.Contacts;

public abstract class Contact : ApplicationNamedEntity
{
    public required string Number { get; set; }
}
