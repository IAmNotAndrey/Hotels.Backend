using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Users;

public class TouristDtoB : ApplicationUserDtoB
{
    [EmailAddress] public string? Email { get; set; }
}
