using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Regions;

public class CityDtoB : ApplicationBaseEntityDtoB
{
    [Required] public Guid CountrySubjectId { get; set; }
}
