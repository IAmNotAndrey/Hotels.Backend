using Hotels.Persistence.Interfaces.Repositories;
using Hotels.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Persistence.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IPartnerRepo, PartnerRepo>();
        services.AddTransient<ITouristRepo, TouristRepo>();
        services.AddTransient<IHousingRepo, HousingRepo>();
        services.AddTransient<IReviewRepo, ReviewRepo>();
        services.AddTransient<ITourRepo, TourRepo>();
        services.AddTransient<ITravelAgentRepo, TravelAgentRepo>();
        services.AddTransient<ITravelAgentSubscriptionRepo, TravelAgentSubscriptionRepo>();
        services.AddTransient<ICountrySubjectRepo, CountrySubjectRepo>();
        services.AddTransient<IRoomRepo, RoomRepo>();
        services.AddTransient<IAttractionRepo, AttractionRepo>();
        services.AddTransient<ICityRepo, CityRepo>();
        services.AddTransient<INearbyRepo, NearbyRepo>();
        services.AddTransient<ICafeSubscriptionRepo, CafeSubscriptionRepo>();
        services.AddTransient<IAdminRepo, AdminRepo>();
        services.AddTransient<IWeekRateRepo, WeekRateRepo>();
        services.AddTransient<IImageStorageRepo, ImageStorageRepo>();
        services.AddTransient(typeof(IGenericRepo<,>), typeof(GenericRepo<,>));

        return services;
    }
}
