using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Contacts;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Infrastructure.Services;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class PartnerServiceTests
{
    private readonly Mock<IGenericRepo<Partner, string>> _mockRepo;
    private readonly Mock<ApplicationContext> _mockContext;
    private readonly PartnerService _partnerService;

    public PartnerServiceTests()
    {
        _mockRepo = new Mock<IGenericRepo<Partner, string>>();

        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;

        _mockContext = new Mock<ApplicationContext>(
            options,
            Mock.Of<IStaticFilesService>())
        {
            CallBase = true
        };

        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _partnerService = new PartnerService(_mockContext.Object, _mockRepo.Object);
    }

    #region SetToModerateAsync

    [Fact]
    public async Task SetToModerateAsync_PartnerExists_SetsAccountStatusToOnModeration()
    {
        // Arrange
        var partnerId = "partner1";
        var partner = new Partner { AccountStatus = AccountStatus.Active };
        _mockRepo.Setup(repo => repo.GetByIdAsync(partnerId, false))
            .ReturnsAsync(partner);

        // Act
        await _partnerService.SetToModerateAsync(partnerId);

        // Assert
        Assert.Equal(AccountStatus.OnModeration, partner.AccountStatus);
        _mockContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task SetToModerateAsync_PartnerDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var partnerId = "test-not-existed-id";
        _mockRepo.Setup(repo => repo.GetByIdAsync(partnerId, false))
            .Throws<EntityNotFoundException>();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _partnerService.SetToModerateAsync(partnerId));
    }

    #endregion

    #region IsValidForModeration

    public static TheoryData<Partner, List<string>> ValidationTestCases => new()
{
    // Valid case
    {
        new Partner
        {
            Name = "Valid Partner",
            TypeId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            Contacts = [new Mock<ApplicationObjectContact>().Object]
        },
        new List<string>()
    },
    // Missing name
    {
        new Partner
        {
            Name = null,
            TypeId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            Contacts = [new Mock<ApplicationObjectContact>().Object]
        },
        new List<string> { "Name is required." }
    },
    // Missing type
    {
        new Partner
        {
            Name = "Partner",
            TypeId = null,
            CityId = Guid.NewGuid(),
            Contacts = [new Mock<ApplicationObjectContact>().Object]
        },
        new List<string> { "TypeId is required." }
    },
    // Missing city
    {
        new Partner
        {
            Name = "Partner",
            TypeId = Guid.NewGuid(),
            CityId = null,
            Contacts = [new Mock<ApplicationObjectContact>().Object]
        },
        new List<string> { "CityId is required." }
    },
    // Missing contacts
    {
        new Partner
        {
            Name = "Partner",
            TypeId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            Contacts = []
        },
        new List<string> { "At least one of Contacts is required." }
    },
    // Multiple errors
    {
        new Partner
        {
            Name = "",
            TypeId = null,
            CityId = null,
            Contacts = []
        },
        new List<string>
        {
            "Name is required.",
            "TypeId is required.",
            "CityId is required.",
            "At least one of Contacts is required."
        }
    }
};

    [Theory]
    [MemberData(nameof(ValidationTestCases))]
    public void IsValidForModeration_ValidParameters_ValidatesCorrectly(Partner partner, List<string> expectedErrors)
    {
        // Act
        var isValid = _partnerService.IsValidForModeration(partner, out var validationErrors);

        // Assert
        Assert.Equal(expectedErrors.Count == 0, isValid);
        Assert.Equal(expectedErrors, validationErrors);
    }

    [Fact]
    public void IsValidForModeration_NullPartner_ThrowsException()
    {
        // Arrange
        Partner partner = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _partnerService.IsValidForModeration(partner, out _));
    }

    #endregion
}
