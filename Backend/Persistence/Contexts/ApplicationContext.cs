using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Common;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Comforts;
using Hotels.Domain.Entities.Contacts;
using Hotels.Domain.Entities.EntityTypes;
using Hotels.Domain.Entities.Feeds;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Domain.Entities.Places;
using Hotels.Domain.Entities.Reviews;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Entities.WeekRates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Contexts;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    private readonly IStaticFilesService _staticFilesService;
    private readonly IContextDefaultValuesSetterService _valuesSetterService;

    // Comforts
    public virtual DbSet<Comfort> Comforts { get; init; }
    public virtual DbSet<ObjectComfort> ObjectComforts { get; init; }
    public virtual DbSet<SubobjectComfort> SubobjectComforts { get; init; }
    // Types
    public virtual DbSet<HousingType> HousingTypes { get; init; }
    public virtual DbSet<ObjectType> ObjectTypes { get; init; }
    public virtual DbSet<RoomType> RoomTypes { get; init; }
    public virtual DbSet<SubobjectType> SubobjectTypes { get; init; }
    public virtual DbSet<TourType> TourTypes { get; init; }
    public virtual DbSet<TypeEntity> TypeEntities { get; init; }
    // Feeds
    public virtual DbSet<Feed> Feeds { get; init; }
    public virtual DbSet<ObjectFeed> ObjectFeeds { get; init; }
    public virtual DbSet<SubobjectFeed> SubobjectFeeds { get; init; }
    // ImageLinks
    public virtual DbSet<TitledImageLink> TitledImageLinks { get; init; }
    public virtual DbSet<ObjectImageLink> ObjectImageLinks { get; init; }
    public virtual DbSet<ReviewImageLink> ReviewImageLinks { get; init; }
    public virtual DbSet<SubobjectImageLink> SubobjectImageLinks { get; init; }
    public virtual DbSet<TourImageLink> TourImageLinks { get; init; }
    public virtual DbSet<TravelAgentLogoLink> TravelAgentLogoLinks { get; init; }
    public virtual DbSet<AttractionImageLink> AttractionImageLinks { get; init; }
    public virtual DbSet<CafeImageLink> CafeImageLinks { get; init; }
    public virtual DbSet<NearbyImageLink> NearbyImageLinks { get; init; }

    public virtual DbSet<CafeMenuFileLink> CafeMenuFileLinks { get; init; }
    // Regions
    public virtual DbSet<City> Cities { get; init; }
    public virtual DbSet<CountrySubject> CountrySubjects { get; init; }
    public virtual DbSet<Region> Regions { get; init; }
    // Subobjects
    public virtual DbSet<Housing> Housings { get; init; }
    public virtual DbSet<Room> Rooms { get; init; }
    public virtual DbSet<Subobject> Subobjects { get; init; }
    // Users
    public virtual DbSet<Admin> Admins { get; init; }
    public virtual DbSet<ApplicationObject> ApplicationObjects { get; init; }
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; init; }
    public virtual DbSet<Partner> Partners { get; init; }
    public virtual DbSet<Tourist> Tourists { get; init; }
    public virtual DbSet<TravelAgent> TravelAgents { get; init; }

    //Contacts
    public virtual DbSet<Contact> Contacts { get; init; }
    public virtual DbSet<ApplicationObjectContact> ApplicationObjectContacts { get; init; }
    public virtual DbSet<CafeContact> CafeContacts { get; init; }


    public virtual DbSet<ApplicationNamedEntity> ApplicationBaseEntities { get; init; }
    public virtual DbSet<Booking> Bookings { get; init; }
    public virtual DbSet<Nearby> Nearbies { get; init; }
    public virtual DbSet<Attraction> Attractions { get; init; }
    public virtual DbSet<Cafe> Cafes { get; init; }

    public virtual DbSet<PartnerReview> ObjectReviews { get; init; }
    public virtual DbSet<TourReview> TourReviews { get; init; }
    public virtual DbSet<Review> Reviews { get; init; }

    public virtual DbSet<Toilet> Toilets { get; init; }
    public virtual DbSet<Bathroom> Bathrooms { get; init; }
    public virtual DbSet<Tour> Tours { get; init; }

    public virtual DbSet<WeekRate> WeekRates { get; init; }
    public virtual DbSet<SubobjectWeekRate> SubobjectWeekRates { get; init; }
    //Subscriptions
    public virtual DbSet<PaidService> PaidServices { get; init; }
    public virtual DbSet<TimeLimitedPaidService> TimeLimitedPaidServices { get; init; }
    public virtual DbSet<TravelAgentSubscription> TravelAgentSubscriptions { get; init; }
    public virtual DbSet<CafeSubscription> CafeSubscriptions { get; init; }
    public virtual DbSet<TravelAgentTimeLimitedPaidService> TravelAgentTimeLimitedPaidServices { get; init; }
    public virtual DbSet<CafeTimeLimitedPaidService> CafeTimeLimitedPaidServices { get; init; }


    public ApplicationContext(DbContextOptions<ApplicationContext> options, IStaticFilesService staticFilesService, IContextDefaultValuesSetterService valuesSetterService) : base(options)
    {
        // NOTE: не переставлять порядок

        _staticFilesService = staticFilesService;
        _valuesSetterService = valuesSetterService;

        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //_valuesSetterService.SetDefaultValues(builder);

        // NOTE: Без явной настройки выдаёт ошибку
        builder.Entity<Subobject>()
            .HasOne(s => s.WeekRate)
            .WithOne(w => w.Subobject)
            .HasForeignKey<SubobjectWeekRate>(w => w.SubobjectId);
        builder.Entity<Cafe>()
            .HasOne(c => c.Menu)
            .WithOne(cmfl => cmfl.Cafe)
            .HasForeignKey<CafeMenuFileLink>(cmfl => cmfl.CafeId);
        builder.Entity<Nearby>()
            .HasOne(n => n.ImageLink)
            .WithOne(nil => nil.Nearby)
            .HasForeignKey<NearbyImageLink>(nil => nil.NearbyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<TravelAgent>()
            .HasMany(ta => ta.Subscriptions)
            .WithOne(s => s.TravelAgent)
            .HasForeignKey(tas => tas.TravelAgentId);
        builder.Entity<TravelAgentTimeLimitedPaidService>()
            .HasMany(ta => ta.TravelAgentSubscriptions)
            .WithOne(s => s.PaidService)
            .HasForeignKey(tas => tas.PaidServiceId);
        builder.Entity<Partner>()
            .HasOne(p => p.City)
            .WithMany(c => c.Partners)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.SetNull);

        // Запрещаем создавать множество отзывов на одну модель одному `Tourist`у
        builder.Entity<TourReview>()
            .HasIndex(tr => new { tr.TouristId, tr.TourId })
            .IsUnique();
        builder.Entity<PartnerReview>()
            .HasIndex(pr => new { pr.TouristId, pr.PartnerId })
            .IsUnique();
        builder.Entity<TravelAgentSubscription>()
            .HasIndex(tas => new { tas.TravelAgentId, tas.PaidServiceId })
            .IsUnique();
        builder.Entity<CountrySubject>()
            .HasIndex(cs => new { cs.Name, cs.CountryCode })
            .IsUnique();
        builder.Entity<CafeSubscription>()
            .HasIndex(cs => new { cs.CafeId, cs.PaidServiceId })
            .IsUnique();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Получаем список удаляемых 'ImageLink'
        ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is StaticFile)
            .Select(e => (StaticFile)e.Entity)
            .ToList()
            .ForEach(sf => _staticFilesService.Remove(sf.Uri)); // Удаляем изображения из 'wwwroot'

        return base.SaveChangesAsync(cancellationToken);
    }
}
