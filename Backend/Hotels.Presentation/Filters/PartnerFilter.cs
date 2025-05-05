using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Presentation.Interfaces;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Hotels.Presentation.Filters;

public class PartnerFilter<TSubobjectFilter> : IFilterModel<Partner> where TSubobjectFilter : IFilterModel<Subobject>
{
    // todo тип жилья?
    public bool PublishedOnly { get; set; } = false;
    public HashSet<Guid> PartnerComfortIds { get; set; } = [];
    public HashSet<Guid> PartnerFeedIds { get; set; } = [];
    public HashSet<Guid> NearbyIds { get; set; } = [];

    public TSubobjectFilter? SubobjectFilter { get; set; }

    [JsonIgnore]
    public Expression<Func<Partner, bool>> FilterExpression => p =>
         // If `PublishedOnly == true` then check, else => don't
         (!PublishedOnly || p.PublicationStatus == PublicationStatus.Published && p.AccountStatus == AccountStatus.Active) &&
         (PartnerComfortIds.Count == 0 || PartnerComfortIds.Any(id => p.Comforts.Any(c => c.Id == id))) &&
         (PartnerFeedIds.Count == 0 || PartnerFeedIds.Any(id => p.Feeds.Any(f => f.Id == id))) &&
         (NearbyIds.Count == 0 || NearbyIds.Any(id => p.Nearbies.Any(n => n.Id == id))) &&
         (SubobjectFilter == null || p.Subobjects.AsQueryable().Any(SubobjectFilter.FilterExpression));

    public void Reset()
    {
        PublishedOnly = false;
        PartnerComfortIds = [];
        PartnerFeedIds = [];
        NearbyIds = [];
        SubobjectFilter.Reset();
    }
}
