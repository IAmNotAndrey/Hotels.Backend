using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.StaticFiles;

namespace Hotels.Application.Dtos;

public class NearbyDto : ApplicationNamedEntityDto
{
    public Guid CountrySubjectId { get; set; }
    public NearbyImageLinkDto? ImageLink { get; set; }
}
