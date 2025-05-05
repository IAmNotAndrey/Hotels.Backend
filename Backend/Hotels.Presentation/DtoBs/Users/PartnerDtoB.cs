using Hotels.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Users;

public class PartnerDtoB : ApplicationObjectDtoB
{
    [Required] public PublicationStatus PublicationStatus { get; set; }

    // ===

    public Guid? TypeId { get; set; }
    public Guid? CityId { get; set; }
}
