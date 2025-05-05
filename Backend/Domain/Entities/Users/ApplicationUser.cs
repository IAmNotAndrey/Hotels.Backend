using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Users;

public abstract class ApplicationUser : IdentityUser, ICreatedAt, IKey<string>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Inactive;
    [NotMapped] public abstract IdentityRole Role { get; }

    /// <summary>
    /// Публичное название
    /// </summary>
    public string? Name { get; set; }
}
