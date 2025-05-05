using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Common;

public class ApplicationBaseEntityDtoB
{
    [Required, Length(3, 128)]
    public virtual required string Name { get; set; }
}
