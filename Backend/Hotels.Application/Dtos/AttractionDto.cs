using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.StaticFiles;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos;

public class AttractionDto : ApplicationNamedEntityDto
{
    public string Description { get; set; } = null!;
    public string? Coordinates { get; set; }
    public PublicationStatus PublicationStatus { get; set; }


    public AttractionImageLinkDto? TitleImageLink { get; set; }

    public Guid CountrySubjectId { get; set; }

    public ICollection<AttractionImageLinkDto> ImageLinks { get; set; } = [];
}
