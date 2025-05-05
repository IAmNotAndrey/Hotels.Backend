using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Presentation.Interfaces;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Hotels.Presentation.Filters;

/// <summary>
/// Partner filter using by admin
/// </summary>
public class PartnerFilter_ForAdminUse<TSubobjectFilter> : IFilterModel<Partner> where TSubobjectFilter : IFilterModel<Subobject>
{
    public bool? IsAdvertised { get; set; }
    public bool? IsPromoSeries { get; set; }
    public AccountStatus? AccountStatus { get; set; }
    public PublicationStatus? PublicationStatus { get; set; }
    public HashSet<Guid> CountrySubjectIds { get; set; } = [];
    public required TSubobjectFilter SubobjectFilter { get; set; }

    [JsonIgnore]
    public Expression<Func<Partner, bool>> FilterExpression => p =>
        (IsAdvertised == null || p.IsAdvertised == IsAdvertised) &&
        (IsPromoSeries == null || p.IsPromoSeries == IsPromoSeries) &&
        (AccountStatus == null || p.AccountStatus == AccountStatus) &&
        (PublicationStatus == null || p.PublicationStatus == PublicationStatus) &&
        (CountrySubjectIds.Count == 0 || p.CityId.HasValue && CountrySubjectIds.Contains(p.City!.CountrySubjectId)) && // FIXME: will it work?
        p.Subobjects.AsQueryable().Any(SubobjectFilter.FilterExpression);


    public void Reset()
    {
        IsAdvertised = null;
        IsPromoSeries = null;
        AccountStatus = null;
        PublicationStatus = null;
        PublicationStatus = null;
        CountrySubjectIds.Clear();
        SubobjectFilter.Reset();
    }
}
