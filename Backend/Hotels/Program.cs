using Hotels.Application.Mappings;
using Hotels.Infrastructure.Extensions;
using Hotels.Infrastructure.Factories;
using Hotels.Persistence.Extensions;
using Hotels.Persistence.Interfaces;
using Hotels.Persistence.Seeds;
using Hotels.Presentation.Attributes;
using Hotels.Presentation.Mappers;
using Hotels.Presentation.Middlewares;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
string[] corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>()!;

// Add services.
builder.Services.AddServices();
// Add repositories.
builder.Services.AddRepositories();

builder.Services.AddScoped<ModelValidationAttribute>();
builder.Services.AddScoped<CustomUserClaimsPrincipalFactory<ApplicationUser>>();
#if DEBUG
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
#endif

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationDtoMapperProfile>();
    cfg.AddProfile<ApplicationDtoBMapperProfile>();
});

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// BUG: once you set `opt.SignIn.RequireConfirmedEmail` to ANY LAST `ApplicationUser` child here in `Program.cs` \
// all user's `RequireConfirmedEmail`-properties will be overwritten.

// User settings
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
//.AddSignInManager<SignInManager<ApplicationUser>>();
builder.Services.AddIdentityCore<Partner>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<Partner>>()
    .AddApiEndpoints()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory<Partner>>();
builder.Services.AddIdentityCore<Tourist>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<Tourist>>()
    .AddApiEndpoints()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory<Tourist>>();
builder.Services.AddIdentityCore<TravelAgent>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<TravelAgent>>()
    .AddApiEndpoints()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory<TravelAgent>>();
builder.Services.AddIdentityCore<Admin>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<Admin>>()
    //.AddApiEndpoints()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory<Admin>>();

// NOTE: The following code should be placed AFTER 'AddIdentity' method.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "Identity.Application";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    options.ExpireTimeSpan = TimeSpan.FromDays(30);

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

#if DEBUG
using (var scope = app.Services.CreateScope())
{
    // FIXME
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    context.Database.EnsureCreated();

    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.InitializeAsync(); // ����� �������������� ���� ������
}
#endif

app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlerMiddleware>(); // ���������� ��������� ����������
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MapControllerRoute(
//	name: "default",
//	pattern: "api/v1/{controller}/{action}"); // ��������� ������� ��� ���� ������������
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});
app.UseStaticFiles();

app.MapGroup("api/v1/partner/").MapIdentityApi<Partner>();
app.MapGroup("api/v1/travel_agent/").MapIdentityApi<TravelAgent>();

app.Run();
