namespace Hotels.Domain.Entities.EntityTypes;

public class TourType : TypeEntity
{
    public ICollection<Tour> Tours { get; set; } = [];
}
