using Hotels.Persistence.Interfaces.Repositories;
using Hotels.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Persistence.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IAdminRepo, AdminRepo>();
        services.AddTransient<IAttractionRepo, AttractionRepo>();
        services.AddTransient<ICafeSubscriptionRepo, CafeSubscriptionRepo>();
        services.AddTransient<ICityRepo, CityRepo>();
        services.AddTransient<ICountrySubjectRepo, CountrySubjectRepo>();
        services.AddTransient(typeof(IGenericRepo<,>), typeof(GenericRepo<,>));
        services.AddTransient<IHousingRepo, HousingRepo>();
        services.AddTransient<IImageRepo, ImageRepo>();
        services.AddTransient<IImageStorageRepo, ImageStorageRepo>();
        services.AddTransient<INearbyRepo, NearbyRepo>();
        services.AddTransient<IPartnerRepo, PartnerRepo>();
        services.AddTransient<IReviewRepo, ReviewRepo>();
        services.AddTransient<IRoomRepo, RoomRepo>();
        services.AddTransient<ITouristRepo, TouristRepo>();
        services.AddTransient<ITourRepo, TourRepo>();
        services.AddTransient<ITravelAgentRepo, TravelAgentRepo>();
        services.AddTransient<ITravelAgentSubscriptionRepo, TravelAgentSubscriptionRepo>();
        services.AddTransient<IWeekRateRepo, WeekRateRepo>();

        return services;
    }
}
