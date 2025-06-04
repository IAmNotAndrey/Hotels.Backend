using Hotels.Application.Interfaces.Services;
using Hotels.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotels.Infrastructure.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<IBathroomService, BathroomService>();
        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<ICafeService, CafeService>();
        services.AddTransient<ICafeSubscriptionService, CafeSubscriptionService>();
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.AddTransient<INearbyService, NearbyService>();
        services.AddTransient<IObjectComfortService, ObjectComfortService>();
        services.AddTransient<IObjectFeedService, ObjectFeedService>();
        services.AddTransient<IPartnerService, PartnerService>();
        services.AddTransient<ISmsSender, SmsAeroSenderService>();
        services.AddTransient<IStaticFilesService, StaticFilesService>();
        services.AddTransient<ISubobjectComfortService, SubobjectComfortService>();
        services.AddTransient<ISubobjectService, SubobjectService>();
        services.AddTransient<IToiletService, ToiletService>();
        services.AddTransient<ITravelAgentSubscriptionService, TravelAgentSubscriptionService>();
        services.AddScoped<IPaymentService<string>, YooKassaService>();

        services.AddHttpClient();

        return services;
    }
}
