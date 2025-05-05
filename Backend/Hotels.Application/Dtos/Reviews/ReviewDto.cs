using Hotels.Application.Dtos.Common;
using Hotels.Application.Dtos.StaticFiles;

namespace Hotels.Application.Dtos.Reviews;

public abstract class ReviewDto : ApplicationNamedEntityDto
{
    public DateTime CreatedAt { get; init; }
    public float Rating { get; set; }
    public string Comment { get; set; } = null!;

    // ===

    public virtual ICollection<ReviewImageLinkDto> ImageLinks { get; set; } = [];
}
