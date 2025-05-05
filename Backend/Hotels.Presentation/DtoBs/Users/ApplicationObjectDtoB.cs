using Hotels.Presentation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Users;

public abstract class ApplicationObjectDtoB : ApplicationUserDtoB
{
    [Length(3, 5000)]
    public string? Description { get; set; }
    public string? Address { get; set; }
    [Coordinates] public string? Coordinates { get; set; }

}
