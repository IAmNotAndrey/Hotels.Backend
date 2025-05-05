using Hotels.Presentation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Regions;

public class CountrySubjectDtoB : RegionDtoB
{
    [Required, CountryCode] public required string CountryCode { get; set; }
}
