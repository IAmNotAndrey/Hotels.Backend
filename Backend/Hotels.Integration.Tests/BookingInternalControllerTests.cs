using FluentAssertions;
using Hotels.BookingsService.Controllers;
using Hotels.Domain.Entities;
using Hotels.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Hotels.Integration.Tests;

public class BookingInternalControllerTests : IClassFixture<WebApplicationFactory<BookingInternalController>>
{
    private const string GetByPaymentIdUrl = "GetByPaymentId/";
    private const string HasBookingConflictWithBookingUrl = "HasBookingConflictWithBooking/";
    private const string HasBookingConflictWithSubobjectUrl = "HasBookingConflictWithSubobject/";

    private readonly Uri _baseAddress = new("https://localhost:5006/api/internal/v1/bookings/");

    private readonly WebApplicationFactory<BookingInternalController> _factory;

    public BookingInternalControllerTests(WebApplicationFactory<BookingInternalController> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptors = services.Where(d =>
                        d.ServiceType == typeof(DbContextOptions<ApplicationContext>)
                     || d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationContext>))
                    .ToArray();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                string dbName = Guid.NewGuid().ToString();
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });
            });
        });
    }

    #region GetByPaymentId
    [Fact]
    public async Task GetByPaymentId_Succeed()
    {
        // Arrange
        var paymentId = "test-payment-123";

        // Add test obejct in the app DB
        Booking testBooking = new()
        {
            Id = Guid.NewGuid(),
            PaymentId = paymentId,
            DateIn = DateOnly.FromDateTime(DateTime.Today),
            DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
            SubobjectId = Guid.NewGuid(),
            TouristId = "tourist-1"
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.Bookings.AddAsync(testBooking);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        string url = GetByPaymentIdUrl + paymentId;

        // Act
        var response = await client.GetAsync(url);
        Booking? result = await response.Content.ReadFromJsonAsync<Booking>();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(testBooking.Id);
        result.PaymentId.Should().Be(testBooking.PaymentId);
        result.DateIn.Should().Be(testBooking.DateIn);
        result.DateOut.Should().Be(testBooking.DateOut);
        result.SubobjectId.Should().Be(testBooking.SubobjectId);
        result.TouristId.Should().Be(testBooking.TouristId);
    }

    [Fact]
    public async Task GetByPaymentId_WhenBookingNotExisting_Returns404()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        string nonExistingPaymentId = Guid.NewGuid().ToString();
        string url = GetByPaymentIdUrl + nonExistingPaymentId;

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    #endregion

    #region HasBookingConflictWithBooking
    [Fact]
    public async Task HasBookingConflictWithBooking_WhenNoDateConflict_ReturnsFalse()
    {
        // Arrange
        Guid testBookingId = Guid.NewGuid();
        Booking testBooking = new()
        {
            Id = testBookingId,
            PaymentId = "",
            DateIn = DateOnly.FromDateTime(DateTime.Today),
            DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
            SubobjectId = Guid.NewGuid(),
            TouristId = ""
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.Bookings.AddAsync(testBooking);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = startDate.AddDays(1);

        string url = $"{HasBookingConflictWithBookingUrl}?bookingId={testBookingId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        // Act
        var response = await client.PostAsync(url, null);
        bool result = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task HasBookingConflictWithBooking_WhenHasDateConflict_ReturnsTrue()
    {
        // Arrange
        Guid testBookingId = Guid.NewGuid();
        Booking testBooking = new()
        {
            Id = testBookingId,
            PaymentId = "",
            DateIn = DateOnly.FromDateTime(DateTime.Today),
            DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
            SubobjectId = Guid.NewGuid(),
            TouristId = ""
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.Bookings.AddAsync(testBooking);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = DateOnly.MaxValue;

        string url = $"{HasBookingConflictWithBookingUrl}?bookingId={testBookingId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        // Act
        var response = await client.PostAsync(url, null);
        bool result = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasBookingConflictWithBooking_WhenBookingNotExisting_Returns404()
    {
        // Arrange
        Guid testBookingId = Guid.NewGuid();
        Booking testBooking = new()
        {
            Id = testBookingId,
            PaymentId = "",
            DateIn = DateOnly.FromDateTime(DateTime.Today),
            DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
            SubobjectId = Guid.NewGuid(),
            TouristId = ""
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.Bookings.AddAsync(testBooking);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        Guid notExistingBookingId = Guid.NewGuid();
        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = DateOnly.MaxValue;

        string url = $"{HasBookingConflictWithBookingUrl}?bookingId={notExistingBookingId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        // Act
        var response = await client.PostAsync(url, null);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    #endregion

    #region HasBookingConflictWithSubobject
    [Fact]
    public async Task HasBookingConflictWithSubobject_WhenNoDateConflict_ReturnsFalse()
    {
        // Arrange
        Guid testSubobjectId = Guid.NewGuid();
        List<Booking> testBookings =
        [
            new()
            {
                Id = Guid.NewGuid(),
                PaymentId = "",
                DateIn = DateOnly.FromDateTime(DateTime.Today),
                DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                SubobjectId = testSubobjectId,
                TouristId = ""
            },
            new()
            {
                Id = Guid.NewGuid(),
                PaymentId = "",
                DateIn = DateOnly.FromDateTime(DateTime.Today.AddDays(4)),
                DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(8)),
                SubobjectId = testSubobjectId,
                TouristId = ""
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.Bookings.AddRangeAsync(testBookings);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = startDate.AddDays(1);

        string url = $"{HasBookingConflictWithSubobjectUrl}?subobjectId={testSubobjectId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        // Act
        var response = await client.PostAsync(url, null);
        bool result = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task HasBookingConflictWithSubobject_WhenHasDateConflict_ReturnsTrue()
    {
        // Arrange
        Guid testSubobjectId = Guid.NewGuid();
        List<Booking> testBookings =
        [
            new()
            {
                Id = Guid.NewGuid(),
                PaymentId = "",
                DateIn = DateOnly.FromDateTime(DateTime.Today),
                DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                SubobjectId = testSubobjectId,
                TouristId = ""
            },
            new()
            {
                Id = Guid.NewGuid(),
                PaymentId = "",
                DateIn = DateOnly.FromDateTime(DateTime.Today.AddDays(4)),
                DateOut = DateOnly.FromDateTime(DateTime.Today.AddDays(8)),
                SubobjectId = testSubobjectId,
                TouristId = ""
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.Bookings.AddRangeAsync(testBookings);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        DateOnly startDate = DateOnly.FromDateTime(DateTime.Today);
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(8));

        string url = $"{HasBookingConflictWithSubobjectUrl}?subobjectId={testSubobjectId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        // Act
        var response = await client.PostAsync(url, null);
        bool result = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasBookingConflictWithSubobject_WhenSubobjectNotExisting_ReturnsFalse()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        Guid notExistingSubobjectId = Guid.NewGuid();
        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = DateOnly.MaxValue;

        string url = $"{HasBookingConflictWithSubobjectUrl}?subobjectId={notExistingSubobjectId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        // Act
        var response = await client.PostAsync(url, null);
        bool result = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        result.Should().BeFalse();
    }
    #endregion
}
