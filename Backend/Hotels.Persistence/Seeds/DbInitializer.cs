using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.Contacts;
using Hotels.Domain.Entities.EntityTypes;
using Hotels.Domain.Entities.Feeds;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.Reviews;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Entities.WeekRates;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hotels.Persistence.Seeds;

public class DbInitializer : IDbInitializer
{
    private const string DefaultUserPassword = "String1!";

    private readonly ApplicationContext _db;
    private readonly UserManager<Partner> _partnerUM;
    private readonly UserManager<Tourist> _touristUM;
    private readonly UserManager<TravelAgent> _travelAgentUM;
    private readonly UserManager<Admin> _adminUM;
    private readonly ILogger<DbInitializer> _logger;
    private readonly IConfiguration _configuration;

    public DbInitializer(
        ApplicationContext db,
        UserManager<Partner> partnerUM,
        UserManager<Tourist> touristUM,
        UserManager<TravelAgent> travelAgentUM,
        UserManager<Admin> adminUM,
        ILogger<DbInitializer> logger,
        IConfiguration configuration)
    {
        _db = db;
        _partnerUM = partnerUM;
        _touristUM = touristUM;
        _travelAgentUM = travelAgentUM;
        _adminUM = adminUM;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        // Обеспечить создание базы данных
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();

        #region Users

        int partnerCount = 13;
        List<Partner> partners = [];

        for (int i = 0; i < partnerCount; i++)
        {
            partners.Add(new()
            {
                Email = $"partner{i}@mail.ru",
                UserName = $"partner{i}@mail.ru",
                EmailConfirmed = true,
            });
        }
        foreach (var partner in partners)
        {
            await _partnerUM.CreateAsync(partner, DefaultUserPassword);
        }

        var tourists = new List<Tourist>
        {
            new() { PhoneNumber = "+70000000000", UserName = "+70000000000" },
            new() { PhoneNumber = "+79999999999", UserName = "+79999999999" },
        };
        foreach (var tourist in tourists)
        {
            await _touristUM.CreateAsync(tourist, DefaultUserPassword);
        }

        var travelAgents = new List<TravelAgent>
        {
            new() { Email = "travelAgent1@mail.ru", UserName = "travelAgent1@mail.ru", EmailConfirmed = true, PublicationStatus = PublicationStatus.Published, AccountStatus = AccountStatus.Active },
            new() { Email = "travelAgent2@mail.ru", UserName = "travelAgent2@mail.ru", EmailConfirmed = true },
        };
        foreach (var ta in travelAgents)
        {
            await _travelAgentUM.CreateAsync(ta, DefaultUserPassword);
        }

        var admins = new List<Admin>
        {
            new() { Email = "admin1@mail.ru", UserName = "admin1@mail.ru", EmailConfirmed = true},
            new() { Email = "admin2@mail.ru", UserName = "admin2@mail.ru", EmailConfirmed = true},
        };
        foreach (var a in admins)
        {
            await _adminUM.CreateAsync(a, DefaultUserPassword);
        }

        #endregion

        #region Subobjects

        var housings = new List<Housing>
        {
            new()
            {
                Name = "Housing 1",
                Description = "Description for Housing 1",
                PartnerId = partners[0].Id,
                Square = 150.0f,
                MinDaysForOrder = 2,
                BedCount = 3,
                Season = SubobjectSeason.Summer,
            },
            new()
            {
                Name = "Housing 2",
                Description = "Description for Housing 2",
                PartnerId = partners[1].Id,
                Square = 200.0f,
                MinDaysForOrder = 3,
                BedCount = 4,
                Season = SubobjectSeason.SummerAndWinter,
            }
        };
        await _db.Housings.AddRangeAsync(housings);
        await _db.SaveChangesAsync();

        #endregion

        #region Comforts

        // Добавление связей 1:M и M:M
        var objComforts = new List<ObjectComfort>
        {
            new() { Name = "ObjectComfort 1" },
            new() { Name = "ObjectComfort 2" },
        };
        //await _db.ObjectComforts.AddRangeAsync(objComforts);
        partners[0].Comforts.Add(objComforts[0]);
        partners[0].Comforts.Add(objComforts[1]);
        partners[1].Comforts.Add(objComforts[1]);

        var subobjectComforts = new List<SubobjectComfort>
        {
            new() { Name = "SubobjectComfort 1" },
            new() { Name = "SubobjectComfort 2" },
        };
        //await _db.SubobjectComforts.AddRangeAsync(subobjectComforts);
        housings[0].Comforts.Add(subobjectComforts[0]);
        housings[0].Comforts.Add(subobjectComforts[1]);
        housings[1].Comforts.Add(subobjectComforts[1]);

        await _db.SaveChangesAsync();

        #endregion

        #region Feeds

        var objFeeds = new List<ObjectFeed>
        {
            new() { Name = "ObjectFeed 1" },
            new() { Name = "ObjectFeed 2" }
        };
        //await _db.ObjectFeeds.AddRangeAsync(objFeeds);
        partners[0].Feeds.Add(objFeeds[0]);
        partners[0].Feeds.Add(objFeeds[1]);
        partners[1].Feeds.Add(objFeeds[1]);

        var subobjectFeeds = new List<SubobjectFeed>
        {
            new() { Name = "SubobjectFeed 1" },
            new() { Name = "SubobjectFeed 2" }
        };
        //await _db.SubobjectFeeds.AddRangeAsync(subobjectFeeds);
        housings[0].Feeds.Add(subobjectFeeds[0]);
        housings[0].Feeds.Add(subobjectFeeds[1]);
        housings[1].Feeds.Add(subobjectFeeds[1]);

        await _db.SaveChangesAsync();

        #endregion

        // TODO ? Images

        #region Places

        var countrySubjects = new List<CountrySubject>()
        {
            new() {Name = "Moscow oblast"},
            new() {Name = "Perm krai"}
        };
        await _db.CountrySubjects.AddRangeAsync(countrySubjects);

        var cities = new List<City>()
        {
            new() { Name = "Москва",   CountrySubjectId = countrySubjects[0].Id},
            new() { Name = "Пермь",    CountrySubjectId = countrySubjects[1].Id},
        };
        await _db.Cities.AddRangeAsync(cities);
        partners[0].City = cities[0];
        partners[1].City = cities[1];
        // TODO : сделать привязку к Nearby

        await _db.SaveChangesAsync();

        #endregion

        #region Bathrooms

        var bathrooms = new List<Bathroom>
        {
            new() { Name = "Bathroom 1" },
            new() { Name = "Bathroom 2" }
        };
        //await _db.Bathrooms.AddRangeAsync(bathrooms);
        housings[0].Bathrooms.Add(bathrooms[0]);
        housings[1].Bathrooms.Add(bathrooms[1]);
        housings[1].Bathrooms.Add(bathrooms[1]);

        await _db.SaveChangesAsync();

        #endregion

        #region Bookings

        var bookings = new List<Booking>
        {
            new() {
                DateIn = DateOnly.ParseExact("28.08.2024", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                DateOut = DateOnly.ParseExact("30.08.2024", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                PaymentId = "null",
                SubobjectId = housings[0].Id,
                TouristId = tourists[0].Id
            },
            new() {
                DateIn = DateOnly.ParseExact("01.01.2000", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                DateOut = DateOnly.ParseExact("01.02.2000", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                PaymentId = "null",
                SubobjectId = housings[1].Id,
                TouristId = tourists[1].Id
            }
        };
        await _db.Bookings.AddRangeAsync(bookings);

        await _db.SaveChangesAsync();

        #endregion

        #region ApplicationObjectContacts

        var applicationObjectContact = new List<ApplicationObjectContact>
        {
            new() { Name = "Contact 1", Number = "+70000000000", ApplicationObjectId = partners[0].Id },
            new() { Name = "Contact 2", Number = "+75555555555", ApplicationObjectId = partners[0].Id },
            new() { Name = "Contact 3", Number = "+79999999999", ApplicationObjectId = partners[1].Id },
        };
        await _db.ApplicationObjectContacts.AddRangeAsync(applicationObjectContact);
        await _db.SaveChangesAsync();

        #endregion

        #region Nearbies

        var nearbies = new List<Nearby>
        {
            new() { Name = "Nearby 1", CountrySubjectId = countrySubjects[0].Id },
            new() { Name = "Nearby 2", CountrySubjectId = countrySubjects[1].Id }
        };
        //await _db.Nearbies.AddRangeAsync(nearbies);
        partners[0].Nearbies.Add(nearbies[0]);
        partners[0].Nearbies.Add(nearbies[1]);
        partners[1].Nearbies.Add(nearbies[1]);

        await _db.SaveChangesAsync();

        #endregion

        #region Reviews

        var reviews = new List<PartnerReview>()
        {
            new() { PartnerId = partners[0].Id, TouristId = tourists[0].Id, Name = "Review 1", Comment = "Comment 1", Rating = 0.5f},
            new() { PartnerId = partners[1].Id, TouristId = tourists[1].Id, Name = "Review 2", Comment = "Comment 2", Rating = 2.5f},
        };
        await _db.ObjectReviews.AddRangeAsync(reviews);

        await _db.SaveChangesAsync();

        #endregion

        #region Toilets

        // Добавляем данные для связи M:M для сущностей Toilets и Bathrooms
        var toilets = new List<Toilet>
        {
            new() { Name = "Toilet 1" },
            new() { Name = "Toilet 2" }
        };
        //await _db.Toilets.AddRangeAsync(toilets);
        housings[0].Toilets.Add(toilets[0]);
        housings[0].Toilets.Add(toilets[1]);
        housings[1].Toilets.Add(toilets[1]);

        await _db.SaveChangesAsync();

        #endregion

        #region Tours

        var tours = new List<Tour>
        {
            new() {Name = "Tour 1",  CountrySubjectId = countrySubjects[0].Id, TravelAgentId = travelAgents[0].Id },
            new() {Name = "Tour 2",  CountrySubjectId = countrySubjects[1].Id, TravelAgentId = travelAgents[1].Id },
        };
        //await _db.Toilets.AddRangeAsync(toilets);
        await _db.Tours.AddAsync(tours[0]);
        await _db.Tours.AddAsync(tours[1]);

        await _db.SaveChangesAsync();

        #endregion

        #region EntityTypes

        var housingTypes = new List<HousingType>
        {
            new() { Name = "HousingType 1" },
            new() { Name = "HousingType 2" },
        };
        //await _db.HousingTypes.AddRangeAsync(housingTypes);
        housings[0].Type = housingTypes[0];
        housings[1].Type = housingTypes[1];

        var objectTypes = new List<ObjectType>
        {
            new() { Name = "ObjectType 1" },
            new() { Name = "ObjectType 2" },
        };
        //await _db.ObjectTypes.AddRangeAsync(objectTypes);
        partners[0].Type = objectTypes[0];
        partners[1].Type = objectTypes[1];

        var tourTypes = new List<TourType>
        {
            new() { Name = "TourType 1" },
            new() { Name = "TourType 2" },
        };
        //await _db.ObjectTypes.AddRangeAsync(objectTypes);
        tours[0].Type = tourTypes[0];
        tours[1].Type = tourTypes[1];

        await _db.SaveChangesAsync();

        #endregion

        #region WeekRates

        var weekRates = new List<SubobjectWeekRate>
        {
            new() {
                Monday = 100.0m,
                Tuesday = 200.0m,
                Wednesday = 300.0m,
                Thursday = 400.0m,
                Friday = 500.0m,
                Saturday = 600.0m,
                Sunday = 700.0m,
                SubobjectId = housings[0].Id
            },
            new() {
                Monday = 100.0m,
                Tuesday = 200.0m,
                Wednesday = 300.0m,
                Thursday = 400.0m,
                Friday = 500.0m,
                Saturday = 600.0m,
                Sunday = 700.0m,
                SubobjectId = housings[1].Id
            },
        };

        await _db.SubobjectWeekRates.AddRangeAsync(weekRates);
        await _db.SaveChangesAsync();

        #endregion

        #region PaidServices

        TravelAgentTimeLimitedPaidService publicationSubscription = new()
        {
            Name = "PublicationSubscription",
            Duration = TimeSpan.FromDays(30),
            Price = 10,
        };
        await _db.TravelAgentTimeLimitedPaidServices.AddAsync(publicationSubscription);
        await _db.SaveChangesAsync();

        TravelAgentSubscription travelAgentSubscription = new()
        {
            TravelAgentId = travelAgents[0].Id,
            PaidServiceId = publicationSubscription.Id,
        };
        await _db.TravelAgentSubscriptions.AddAsync(travelAgentSubscription);
        await _db.SaveChangesAsync();

        #endregion

        #region Attractions

        List<Attraction> attractions = [
            new() { Name = "Attraction 1", CountrySubjectId = countrySubjects[0].Id },
            new() { Name = "Attraction 2", CountrySubjectId = countrySubjects[1].Id },
        ];
        await _db.Attractions.AddRangeAsync(attractions);
        await _db.SaveChangesAsync();

        #endregion

        #region Cafes

        List<Cafe> cafes = [
            new() { Name = "Cafe 1", CountrySubject = countrySubjects[0] },
            new() { Name = "Cafe 2", CountrySubject = countrySubjects[1] },
        ];
        await _db.Cafes.AddRangeAsync(cafes);
        await _db.SaveChangesAsync();

        #endregion

        #region CafeContacts

        List<CafeContact> cafeContacts = [
            new() { Name = "Cafe Contact 1", Number = "+70000000000", CafeId = cafes[0].Id },
            new() { Name = "Cafe Contact 2", Number = "+75555555555", CafeId = cafes[0].Id },
            new() { Name = "Cafe Contact 3", Number = "+79999999999", CafeId = cafes[1].Id },
        ];
        await _db.CafeContacts.AddRangeAsync(cafeContacts);
        await _db.SaveChangesAsync();

        #endregion

        _logger.LogInformation("The test data has been successfully loaded into '{DbName}'.", nameof(ApplicationContext));
    }
}
