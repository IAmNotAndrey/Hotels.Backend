using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Contacts;

public class ApplicationObjectContact : Contact
{
    public required string ApplicationObjectId { get; set; }
    public ApplicationObject ApplicationObject { get; set; } = null!;
}
