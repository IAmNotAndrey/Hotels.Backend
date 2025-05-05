using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Users;

public abstract class ApplicationUserDtoB
{
    [Length(5, 128)]
    public string? Name { get; set; }
}
