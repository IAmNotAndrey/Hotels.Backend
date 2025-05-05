using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Contacts;

public abstract class ContactDtoB : ApplicationBaseEntityDtoB
{
    [Required, Phone]
    public required string Number { get; set; }
}
