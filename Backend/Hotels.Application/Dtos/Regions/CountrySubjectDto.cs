using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos.Regions;

public class CountrySubjectDto : ApplicationNamedEntityDto
{
    public string CountryCode { get; set; } = null!;
}
