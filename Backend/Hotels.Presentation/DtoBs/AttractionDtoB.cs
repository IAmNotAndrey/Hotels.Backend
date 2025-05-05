using Hotels.Domain.Enums;
using Hotels.Presentation.Attributes;
using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public class AttractionDtoB : ApplicationBaseEntityDtoB
{
    [Required, Length(3, 5000)] public string Description { get; set; } = null!;
    [Coordinates] public string? Coordinates { get; set; }
    [Required] public PublicationStatus PublicationStatus { get; set; }

    [Required] public Guid CountrySubjectId { get; set; }
}
