using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos.Contacts;

public abstract class ContactDto : ApplicationNamedEntityDto
{
    public string Number { get; set; } = null!;
}
