namespace Hotels.Domain.Entities.Contacts;

public class CafeContact : Contact
{
    public required Guid CafeId { get; set; }
    public Cafe Cafe { get; set; } = null!;
}
