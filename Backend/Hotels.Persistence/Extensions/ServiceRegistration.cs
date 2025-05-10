using Hotels.Application.Interfaces.Services;
using Hotels.Infrastructure.Services;
using Hotels.Persistence.Interfaces.Repositories;
using Hotels.Persistence.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Persistence.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IPartnerRepo, PartnerRepo>();
        services.AddTransient<ITouristRepo, TouristRepo>();
        services.AddTransient<IHousingRepo, HousingRepo>();
        services.AddTransient<IReviewRepo, ReviewRepo>();
        services.AddTransient<ITourRepo, TourRepo>();
        services.AddTransient<ITravelAgentRepo, TravelAgentRepo>();
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<ISubobjectRepo, SubobjectRepo>();
        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<ITravelAgentSubscriptionRepo, TravelAgentSubscriptionRepo>();
        services.AddTransient<ISmsSender, SmsAeroSenderService>();
        services.AddTransient<ICountrySubjectRepo, CountrySubjectRepo>();
        services.AddTransient<IRoomRepo, RoomRepo>();
        services.AddTransient<IAttractionRepo, AttractionRepo>();
        services.AddTransient<ICafeRepo, CafeRepo>();
        services.AddTransient<ICityRepo, CityRepo>();
        services.AddTransient<INearbyRepo, NearbyRepo>();
        services.AddTransient<ICafeSubscriptionRepo, CafeSubscriptionRepo>();
        services.AddTransient<IAdminRepo, AdminRepo>();
        services.AddTransient<IObjectComfortRepo, ObjectComfortRepo>();
        services.AddTransient<ISubobjectComfortRepo, SubobjectComfortRepo>();
        services.AddTransient<IBathroomService, BathroomService>();
        services.AddTransient<IObjectFeedRepo, ObjectFeedRepo>();
        services.AddTransient<IToiletRepo, ToiletRepo>();
        services.AddTransient<IWeekRateRepo, WeekRateRepo>();
        services.AddTransient<IImageStorageRepo, ImageStorageRepo>();
        services.AddTransient(typeof(IGenericRepo<,>), typeof(GenericRepo<,>));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.AddTransient<IImageRepo, ImageRepo>();
        services.AddTransient<IStaticFilesService, StaticFilesService>();
        services.AddTransient<ISmsSender, SmsAeroSenderService>();
        services.AddScoped<IPaymentService<string>, YooKassaService>();

        return services;
    }
}
