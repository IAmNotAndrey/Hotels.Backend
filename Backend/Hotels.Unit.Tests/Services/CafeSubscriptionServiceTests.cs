using Hotels.Application.Exceptions;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.PaidServices;
using Hotels.Infrastructure.Services;
using Hotels.Persistence.Interfaces.Repositories;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class CafeSubscriptionServiceTests
{
    private readonly Mock<IGenericRepo<Cafe, Guid>> _cafeRepoMock;
    private readonly Mock<IGenericRepo<CafeTimeLimitedPaidService, Guid>> _cafeTLPSRepoMock;
    private readonly Mock<IGenericRepo<CafeSubscription, Guid>> _cafeSubscriptionRepoMock;
    private readonly CafeSubscriptionService _service;

    public CafeSubscriptionServiceTests()
    {
        _cafeRepoMock = new Mock<IGenericRepo<Cafe, Guid>>();
        _cafeTLPSRepoMock = new Mock<IGenericRepo<CafeTimeLimitedPaidService, Guid>>();
        _cafeSubscriptionRepoMock = new Mock<IGenericRepo<CafeSubscription, Guid>>();

        _service = new CafeSubscriptionService(
            _cafeRepoMock.Object,
            _cafeTLPSRepoMock.Object,
            _cafeSubscriptionRepoMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_CafeDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var cafeId = Guid.NewGuid();
        var paidServiceId = Guid.NewGuid();

        _cafeRepoMock
            .Setup(repo => repo.ExistsAsync(cafeId))
            .ReturnsAsync(false);
        _cafeTLPSRepoMock
            .Setup(repo => repo.ExistsAsync(cafeId))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.CreateAsync(cafeId, paidServiceId));
    }

    [Fact]
    public async Task CreateAsync_PaidServiceDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var cafeId = Guid.NewGuid();
        var paidServiceId = Guid.NewGuid();

        _cafeRepoMock
            .Setup(repo => repo.ExistsAsync(cafeId))
            .ReturnsAsync(true);

        _cafeTLPSRepoMock
            .Setup(repo => repo.ExistsAsync(paidServiceId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.CreateAsync(cafeId, paidServiceId));
    }

    [Fact]
    public async Task CreateAsync_CafeAndPaidServiceDoNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var cafeId = Guid.NewGuid();
        var paidServiceId = Guid.NewGuid();

        _cafeRepoMock
            .Setup(repo => repo.ExistsAsync(cafeId))
            .ReturnsAsync(false);

        _cafeTLPSRepoMock
            .Setup(repo => repo.ExistsAsync(paidServiceId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.CreateAsync(cafeId, paidServiceId));
    }

    [Fact]
    public async Task CreateAsync_CafeAndPaidServiceExist_CreatesCafeSubscription()
    {
        // Arrange
        var cafeId = Guid.NewGuid();
        var paidServiceId = Guid.NewGuid();

        _cafeRepoMock
            .Setup(repo => repo.ExistsAsync(cafeId))
            .ReturnsAsync(true);

        _cafeTLPSRepoMock
            .Setup(repo => repo.ExistsAsync(paidServiceId))
            .ReturnsAsync(true);

        var expectedSubscription = new CafeSubscription
        {
            CafeId = cafeId,
            PaidServiceId = paidServiceId
        };

        _cafeSubscriptionRepoMock
            .Setup(repo => repo.AddAsync(It.IsAny<CafeSubscription>()))
            .ReturnsAsync(expectedSubscription);

        // Act
        var result = await _service.CreateAsync(cafeId, paidServiceId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cafeId, result.CafeId);
        Assert.Equal(paidServiceId, result.PaidServiceId);
    }
}
