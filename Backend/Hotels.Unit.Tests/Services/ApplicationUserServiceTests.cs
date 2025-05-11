using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Users;
using Hotels.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace Hotels.Unit.Tests.Services;

public class ApplicationUserServiceTests
{
    private readonly Mock<ClaimsPrincipal> _claimsPrincipalMock = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
    private readonly ApplicationUserService _service;

    public ApplicationUserServiceTests()
    {
        _service = new ApplicationUserService(_userManagerMock.Object);
    }

    #region ChangeName
    [Fact]
    public async Task ChangeNameAsync_UserExists_UpdatesName()
    {
        // Arrange
        string userId = "test-id";
        string newName = "test-name";
        var user = new Mock<ApplicationUser>();

        _userManagerMock.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user.Object);
        _userManagerMock.Setup(um => um.UpdateAsync(user.Object))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.ChangeNameAsync(userId, newName);

        // Assert
        Assert.Equal(newName, user.Object.Name);
        _userManagerMock.Verify(um => um.UpdateAsync(user.Object), Times.Once);
    }

    [Fact]
    public async Task ChangeNameAsync_UserNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        string userId = "test-id";
        _userManagerMock.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.ChangeNameAsync(userId, "test-name"));
    }
    #endregion

    #region IsUserAllowedAsync
    [Fact]
    public async Task IsUserAllowedAsync_RequesterIsAdmin_ReturnsTrue()
    {
        // Arrange
        var admin = new Mock<Admin>();
        _userManagerMock.Setup(um => um.GetUserAsync(_claimsPrincipalMock.Object))
            .ReturnsAsync(admin.Object);

        // Act
        bool result = await _service.IsUserAllowedAsync(_claimsPrincipalMock.Object, "test-id");

        // Assert
        Assert.True(result);
        _userManagerMock.Verify(um => um.GetUserAsync(_claimsPrincipalMock.Object), Times.Once);
    }

    [Fact]
    public async Task IsUserAllowedAsync_RequesterIsNotAdminButIdsMatch_ReturnsTrue()
    {
        // Arrange
        var userId = "test-id";
        var user = new Mock<ApplicationUser>();

        user.Setup(u => u.Id).Returns(userId);
        var claimsPrincipal = new ClaimsPrincipal();

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal))
            .ReturnsAsync(user.Object);

        // Act
        bool result = await _service.IsUserAllowedAsync(claimsPrincipal, userId);

        // Assert
        Assert.True(result);
        _userManagerMock.Verify(um => um.GetUserAsync(claimsPrincipal), Times.Once);
    }

    [Fact]
    public async Task IsUserAllowedAsync_RequesterIsNotAdminAndIdsDoNotMatch_ReturnsFalse()
    {
        // Arrange
        var userId = "test-id";
        var user = new Mock<ApplicationUser>();

        user.Setup(u => u.Id).Returns("other-id");
        var claimsPrincipal = new ClaimsPrincipal();

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal))
            .ReturnsAsync(user.Object);

        // Act
        bool result = await _service.IsUserAllowedAsync(claimsPrincipal, userId);

        // Assert
        Assert.False(result);
        _userManagerMock.Verify(um => um.GetUserAsync(claimsPrincipal), Times.Once);
    }

    [Fact]
    public async Task IsUserAllowedAsync_RequesterIsAdminAndIdsDoNotMatch_ReturnsTrue()
    {
        // Arrange
        var userId = "test-id";
        var user = new Mock<Admin>();

        user.Setup(u => u.Id).Returns("other-id");
        var claimsPrincipal = new ClaimsPrincipal();

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal))
            .ReturnsAsync(user.Object);

        // Act
        bool result = await _service.IsUserAllowedAsync(claimsPrincipal, userId);

        // Assert
        Assert.True(result);
        _userManagerMock.Verify(um => um.GetUserAsync(claimsPrincipal), Times.Once);
    }

    [Fact]
    public async Task IsUserAllowedAsync_RequesterIsNotFound_ReturnsFalse()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal();
        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _service.IsUserAllowedAsync(claimsPrincipal, "test-id");

        // Assert
        Assert.False(result);
    }
    #endregion
}
