using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Users;

public class Admin : ApplicationUser
{
    [NotMapped] public override IdentityRole Role => new(nameof(Admin));
}
