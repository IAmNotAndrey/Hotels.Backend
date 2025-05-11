using Hotels.Domain.Entities.PaidServices;
using Hotels.Infrastructure.Services;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class TravelAgentSubscriptionServiceTests
{
    private readonly Mock<IGenericRepo<TravelAgentSubscription, Guid>> _mockRepo;
    private readonly TravelAgentSubscriptionService _service;

    public TravelAgentSubscriptionServiceTests()
    {
        _mockRepo = new();
        _service = new(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidParameters_CreatesSubscription()
    {
        // Arrange
        var travelAgentId = "test-travelAgent-id";
        var paidServiceId = Guid.NewGuid();
        var expectedSubscription = new TravelAgentSubscription
        {
            TravelAgentId = travelAgentId,
            PaidServiceId = paidServiceId
        };

        _mockRepo
            .Setup(repo => repo.AddAsync(It.IsAny<TravelAgentSubscription>()))
            .ReturnsAsync((TravelAgentSubscription sub) => sub);

        // Act
        var result = await _service.CreateAsync(travelAgentId, paidServiceId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(travelAgentId, result.TravelAgentId);
        Assert.Equal(paidServiceId, result.PaidServiceId);

        _mockRepo.Verify(repo => repo.AddAsync(It.Is<TravelAgentSubscription>(
            sub => sub.TravelAgentId == travelAgentId && sub.PaidServiceId == paidServiceId)
        ), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DatabaseCannotCreateSubscription_ThrowsException()
    {
        // Arrange
        var travelAgentId = "agent123";
        var paidServiceId = Guid.NewGuid();

        _mockRepo
            .Setup(repo => repo.AddAsync(It.IsAny<TravelAgentSubscription>()))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => _service.CreateAsync(travelAgentId, paidServiceId));
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<TravelAgentSubscription>()), Times.Once);
    }
}
