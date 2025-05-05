using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public class NearbyDtoB : ApplicationBaseEntityDtoB
{
    [Required] public required Guid CountrySubjectId { get; set; }
}
