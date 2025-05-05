using Hotels.Application.Dtos.Comforts;
using Hotels.Application.Dtos.Feeds;
using Hotels.Application.Dtos.Reviews;
using Hotels.Application.Dtos.Subobjects;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos.Users;

public class PartnerDto : ApplicationObjectDto
{
    public string? Email { get; set; }
    public SubobjectSeason Season { get; set; }

    public decimal? MinimalWeekRate { get; set; }
    public float? AverageRating { get; set; }

    // ===

    public Guid? TypeId { get; set; }
    public Guid? CityId { get; set; }
    public ICollection<ObjectComfortDto> Comforts { get; set; } = [];
    public ICollection<ObjectFeedDto> Feeds { get; set; } = [];
    public ICollection<NearbyDto> Nearbies { get; set; } = [];
    public ICollection<SubobjectDto> Subobjects { get; set; } = [];
    public ICollection<PartnerReviewDto> Reviews { get; set; } = [];
}
