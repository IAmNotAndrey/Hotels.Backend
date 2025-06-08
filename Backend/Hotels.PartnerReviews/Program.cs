using Hotels.Application.Interfaces.Services;
using Hotels.Application.Mappings;
using Hotels.Infrastructure.Services;
using Hotels.PartnerReviews;
using Hotels.PartnerReviews.Persistence.Interfaces.Repositories;
using Hotels.PartnerReviews.Persistence.Repositories;
using Hotels.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add repositories
builder.Services.AddTransient<IReviewRepo, ReviewRepo>();
builder.Services.AddTransient(typeof(IGenericRepo<,>), typeof(GenericRepo<,>));

// Add services
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
builder.Services.AddScoped<DbContext>(provider =>
    provider.GetRequiredService<ApplicationContext>());

builder.Services.AddTransient<IStaticFilesService, StaticFilesService>(); // TODO : Remove

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationDtoMapperProfile>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
