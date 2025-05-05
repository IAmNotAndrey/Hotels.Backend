using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Presentation.Interfaces;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Hotels.Presentation.Filters;

public class TravelAgentFilter : IFilterModel<TravelAgent>
{
    public bool PublishedOnly { get; set; } = false;
    public HashSet<Guid> SubscriptionsIds { get; set; } = [];

    [JsonIgnore]
    public Expression<Func<TravelAgent, bool>> FilterExpression => ta =>
        (!PublishedOnly || ta.PublicationStatus == PublicationStatus.Published && ta.AccountStatus == AccountStatus.Active) &&
        (SubscriptionsIds.Count == 0 || SubscriptionsIds.Any(id => ta.Subscriptions.Any(sub => sub.Id == id)));

    public void Reset()
    {
        PublishedOnly = false;
    }
}
