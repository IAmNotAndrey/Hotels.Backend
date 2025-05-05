using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Enums;
using Hotels.Presentation.Attributes;
using Hotels.Presentation.Interfaces;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Hotels.Presentation.Filters;

public class SubobjectFilter : IFilterModel<Subobject>
{
    private const int BedCountBorder = 6;
    private const int BedroomCountBorder = 6;

    public HashSet<SubobjectSeason> Seasons { get; set; } = [];
    public HashSet<string> SubobjectChildrenTypeNames { get; set; } = [];
    public HashSet<int> BedCounts { get; set; } = [];
    public HashSet<int> BedroomCounts { get; set; } = [];
    public Dictionary<GuestType, int> Guests { get; set; } = [];

    public DateOnly? DateIn { get; set; }
    public DateOnly? DateOut { get; set; }

    public bool MoreOrEqualBedsThanBorder { get; set; } // 6 и более кроватей
    public bool MoreOrEqualBedroomsThanBorder { get; set; } // 6 и более спальней


    // fixme? Переделать модель Subobjects?
    //public HashSet<int> BedroomCounts { get; set; } = [];
    //public bool AllowedBedroomBorderCount { get; set; } // 6 и более спален

    public HashSet<Guid> SubobjectComfortIds { get; set; } = [];
    public HashSet<Guid> SubobjectFeedIds { get; set; } = [];
    public HashSet<Guid> ToiletIds { get; set; } = [];
    public HashSet<Guid> BathroomIds { get; set; } = [];

    [JsonIgnore]
    public Expression<Func<Subobject, bool>> FilterExpression => so =>
        // If no specific seasons are defined, include all subobjects. Otherwise, filter by the specified seasons.
        (Seasons.Count == 0 || Seasons.Contains(so.Season)) &&

        // If no specific type names are defined, include all subobjects. Otherwise, filter by the specified type names.
        (SubobjectChildrenTypeNames.Count == 0 || SubobjectChildrenTypeNames.Contains(so.TypeName)) &&

        // Filter by bed count: 
        // - If no specific bed counts are defined, include all subobjects. 
        // - Otherwise, include subobjects that match the specified bed counts.
        // - If `MoreOrEqualBedsThanBorder` is true, include subobjects with a bed count greater than or equal to the threshold.
        (!MoreOrEqualBedsThanBorder && BedCounts.Count == 0 ||
         BedCounts.Contains(so.BedCount) ||
         MoreOrEqualBedsThanBorder && so.BedCount >= BedCountBorder) &&

        // Similar to the bed count logic but applied to bedroom count.
        (!MoreOrEqualBedroomsThanBorder && BedroomCounts.Count == 0 ||
         BedroomCounts.Contains(so.BedroomCount) ||
         MoreOrEqualBedroomsThanBorder && so.BedroomCount >= BedroomCountBorder) &&

        // Filter by comforts: If no specific comfort IDs are defined, include all subobjects.
        // Otherwise, include subobjects that contain at least one of the specified comfort IDs.
        (SubobjectComfortIds.Count == 0 ||
         SubobjectComfortIds.Any(id => so.Comforts.Any(sc => sc.Id == id))) &&

        // Filter by feeds: Similar to comforts, but applied to feed IDs.
        (SubobjectFeedIds.Count == 0 ||
         SubobjectFeedIds.Any(id => so.Feeds.Any(f => f.Id == id))) &&

        // Filter by toilets: Include all if no specific toilet IDs are defined. Otherwise, include subobjects that contain at least one of the specified toilet IDs.
        (ToiletIds.Count == 0 ||
         ToiletIds.Any(id => so.Toilets.Any(t => t.Id == id))) &&

        // Filter by bathrooms: Same logic as toilets but applied to bathroom IDs.
        (BathroomIds.Count == 0 ||
         BathroomIds.Any(id => so.Bathrooms.Any(b => b.Id == id))) &&

        // Guest filtering logic:
        (
            Guests.Count == 0 || // If no guest constraints are defined, include all subobjects.
            (
                // If no child or adult guest types are specified, accept the subobject.
                (!Guests.ContainsKey(GuestType.Child) && !Guests.ContainsKey(GuestType.Adult)) ||

                // If both child and adult guest counts are defined, ensure their sum matches the subobject's bed count.
                (Guests.ContainsKey(GuestType.Child) && Guests.ContainsKey(GuestType.Adult) &&
                 (Guests[GuestType.Child] + Guests[GuestType.Adult] == so.BedCount))
            ) &&
            // If baby guest type is specified, the subobject must have extra places available.
            (!Guests.ContainsKey(GuestType.Baby) || so.ExtraPlacePrice != null)
        ) &&

        // DateIn/DateOut booking logic:
        (
            // Case 1: Ensure DateOut is not specified without DateIn; return false if the condition is invalid.
            !(DateOut != null && DateIn == null) &&
            (
                // Case 2: If no dates are specified, include all subobjects.
                DateIn == null && DateOut == null ||

                // Case 3: If only DateIn is specified, check availability for that day.
                (DateIn != null && DateOut == null &&
                    so.Bookings.All(b => DateIn.Value < b.DateIn || DateIn.Value >= b.DateOut)) ||

                // Case 4: If both DateIn and DateOut are specified, check availability for the entire date range.
                (DateIn != null && DateOut != null &&
                     so.Bookings.All(b => DateOut <= b.DateIn || DateIn >= b.DateOut))
             )
        );


    public void Reset()
    {
        Seasons = [];
        SubobjectChildrenTypeNames = [];
        BedCounts = [];
        BedroomCounts = [];
        MoreOrEqualBedsThanBorder = false;
        MoreOrEqualBedroomsThanBorder = false;
        SubobjectComfortIds = [];
        SubobjectFeedIds = [];
        ToiletIds = [];
        BathroomIds = [];
        Guests.Clear();
        DateIn = null;
        DateOut = null;
    }
}
