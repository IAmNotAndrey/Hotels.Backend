using Hotels.Domain.Entities;
using Hotels.Presentation.Interfaces;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Hotels.Presentation.Filters;

public class CafeFilter : IFilterModel<Cafe>
{
    public HashSet<Guid> SubscriptionsIds { get; set; } = [];

    [JsonIgnore]
    public Expression<Func<Cafe, bool>> FilterExpression => c =>
        SubscriptionsIds.Count == 0 || SubscriptionsIds.Any(id => c.Subscriptions.Any(sub => sub.Id == id));

    public void Reset()
    {
        SubscriptionsIds.Clear();
    }
}
