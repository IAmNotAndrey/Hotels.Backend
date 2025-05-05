using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos.Users;

public abstract class ApplicationUserDto
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public AccountStatus AccountStatus { get; set; }
}
