using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities.Places;

public class CountrySubject : Region
{
    public string CountryCode { get; set; } = "ru";

    // ===

    public ICollection<City> Cities { get; set; } = [];
    public ICollection<Nearby> Nearbies { get; set; } = [];
    public ICollection<TravelAgent> TravelAgents { get; set; } = [];
    public ICollection<Tour> Tours { get; set; } = [];
    public ICollection<Attraction> Attractions { get; set; } = [];
    public ICollection<Cafe> Cafes { get; set; } = [];
}
