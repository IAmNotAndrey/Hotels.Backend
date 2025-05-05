using Hotels.Application.Dtos.Contacts;
using Hotels.Application.Dtos.StaticFiles;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos.Users;

public abstract class ApplicationObjectDto : ApplicationUserDto
{
    public string? Description { get; set; }
    public string? Coordinates { get; set; }
    public string? Address { get; set; }

    public bool IsPromoSeries { get; set; }
    public bool IsAdvertised { get; set; }

    public PublicationStatus PublicationStatus { get; set; }
    public ObjectImageLinkDto? TitleImageLink { get; set; }


    // ===

    public ICollection<ApplicationObjectContactDto> Contacts { get; set; } = [];
    public ICollection<ObjectImageLinkDto> ImageLinks { get; set; } = [];
}
