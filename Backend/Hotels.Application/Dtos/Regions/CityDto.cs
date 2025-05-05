using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos.Regions;

public class CityDto : ApplicationNamedEntityDto
{
    public Guid CountrySubjectId { get; set; }
}
