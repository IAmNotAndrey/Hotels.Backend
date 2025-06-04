using Hotels.Application.Exceptions;
using Hotels.Bookings.Infrastructure.Services;
using Hotels.Bookings.Persistence.Interfaces.Repositories;
using Hotels.Domain.Entities;
using Hotels.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class BookingServiceTests
{
    private readonly Mock<IGenericRepo<Booking, Guid>> _mockBookingGenericRepo = new();
    private readonly Mock<IBookingRepo> _mockBookingRepo = new();

    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
        .UseInMemoryDatabase(databaseName: "Test_Database")
        .Options;

        _bookingService = new BookingService(
            _mockBookingGenericRepo.Object,
            _mockBookingRepo.Object
        );
    }

    [Theory]
    [InlineData("2023-10-05", "2023-10-10")]
    [InlineData("2023-10-05", "2023-10-11")]
    [InlineData("2023-10-12", "2023-10-13")]
    [InlineData("2023-10-11", "2023-10-14")]
    [InlineData("2023-10-10", "2023-10-15")]
    [InlineData("2023-10-13", "2023-10-20")]
    [InlineData("2023-10-14", "2023-10-20")]
    public async Task HasBookingConflictAsync_OverlappingDates_ReturnsTrue(
        string newStartStr, string newEndStr)
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var existingBooking = new Booking
        {
            Id = bookingId,
            DateIn = DateOnly.Parse("2023-10-10"),
            DateOut = DateOnly.Parse("2023-10-15")
        };

        _mockBookingGenericRepo.Setup(repo => repo.GetByIdAsync(bookingId, true))
            .ReturnsAsync(existingBooking);

        // Act
        var result = await _bookingService.HasBookingConflictWithBookingAsync(
            bookingId,
            DateOnly.Parse(newStartStr),
            DateOnly.Parse(newEndStr));

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("2023-10-05", "2023-10-08")]
    [InlineData("2023-10-05", "2023-10-09")]
    [InlineData("2023-10-16", "2023-10-20")]
    [InlineData("2024-01-01", "2024-01-30")]
    public async Task HasBookingConflictAsync_NonOverlappingDates_ReturnsFalse(
        string newStartStr, string newEndStr)
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var existingBooking = new Booking
        {
            Id = bookingId,
            DateIn = DateOnly.Parse("2023-10-10"),
            DateOut = DateOnly.Parse("2023-10-15")
        };

        _mockBookingGenericRepo.Setup(repo => repo.GetByIdAsync(bookingId, true))
            .ReturnsAsync(existingBooking);

        // Act
        var result = await _bookingService.HasBookingConflictWithBookingAsync(
            bookingId,
            DateOnly.Parse(newStartStr),
            DateOnly.Parse(newEndStr));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HasBookingConflictAsync_BookingDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var newStartDate = new DateOnly(2023, 10, 6);
        var newEndDate = new DateOnly(2023, 10, 10);

        _mockBookingGenericRepo.Setup(repo => repo.GetByIdAsync(bookingId, true))
            .ThrowsAsync(new EntityNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _bookingService.HasBookingConflictWithBookingAsync(bookingId, newStartDate, newEndDate));
    }
}
