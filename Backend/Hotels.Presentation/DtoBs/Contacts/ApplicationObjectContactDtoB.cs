using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Contacts;

public class ApplicationObjectContactDtoB : ContactDtoB
{
    [Required] public required string ApplicationObjectId { get; set; }
}
