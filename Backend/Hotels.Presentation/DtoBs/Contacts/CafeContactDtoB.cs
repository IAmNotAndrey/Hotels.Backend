using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Contacts;

public class CafeContactDtoB : ContactDtoB
{
    [Required] public required Guid CafeId { get; set; }
}
