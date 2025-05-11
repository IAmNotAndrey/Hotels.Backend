using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Infrastructure.Services;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class AdminServiceTests
{
    private readonly Mock<IGenericRepo<Admin, string>> _mockRepo;
    private readonly Mock<UserManager<Admin>> _mockUserManager;
    private readonly Mock<ApplicationContext> _mockContext;
    private readonly AdminService _adminService;

    public AdminServiceTests()
    {
        _mockRepo = new Mock<IGenericRepo<Admin, string>>();
        _mockUserManager = MockUserManager<Admin>();

        // Исправленная инициализация ApplicationContext
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;

        _mockContext = new Mock<ApplicationContext>(
            options,
            Mock.Of<IStaticFilesService>())
        {
            CallBase = true
        };

        // Мокируем SaveChangesAsync
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _adminService = new AdminService(
            _mockContext.Object,
            _mockUserManager.Object,
            _mockRepo.Object);
    }

    #region ConfirmModerationAsync
    [Fact]
    public async Task ConfirmModerationAsync_ValidAdmin_UpdatesStatusAndSaves()
    {
        // Arrange
        var adminId = "test-id";
        var admin = new Admin { Id = adminId, AccountStatus = AccountStatus.OnModeration };

        _mockRepo.Setup(r => r.GetByIdAsync(adminId, false))
            .ReturnsAsync(admin);

        // Act
        await _adminService.ConfirmModerationAsync(adminId);

        // Assert
        Assert.Equal(AccountStatus.Active, admin.AccountStatus);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ConfirmModerationAsync_AdminNotFound_ThrowsException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), false))
           .Throws<EntityNotFoundException>();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(()
            => _adminService.ConfirmModerationAsync("any-invalid-id"));
    }
    #endregion

    #region CreateAsync
    [Fact]
    public async Task CreateAsync_ValidData_CreatesAdminWithCorrectParameters()
    {
        // Arrange
        const string email = "test@example.com";
        const string password = "Password1!";

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<Admin>(), password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _adminService.CreateAsync(email, password);

        // Assert
        _mockUserManager.Verify(u => u.CreateAsync(
            It.Is<Admin>(a => a.Email == email && a.EmailConfirmed),
            password
        ), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CreationFailed_ThrowsException()
    {
        // Arrange
        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<Admin>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(()
            => _adminService.CreateAsync("invalid", "password"));
    }
    #endregion

    #region DeleteAsync
    [Fact]
    public async Task DeleteAsync_LastAdmin_ThrowsInvalidOperation()
    {
        // Arrange
        var admin = new Admin { Id = "test-id" };
        var users = new List<Admin> { admin }.AsQueryable();

        _mockUserManager.Setup(u => u.FindByIdAsync("test-id"))
            .ReturnsAsync(admin);
        _mockUserManager.Setup(u => u.Users)
            .Returns(users);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(()
            => _adminService.DeleteAsync("test-id"));
        _mockUserManager.Verify(u => u.DeleteAsync(admin), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidAdmin_DeletesSuccessfully()
    {
        // Arrange
        var admin = new Admin { Id = "1" };
        var users = new List<Admin> { admin, new() { Id = "2" } }.AsQueryable();

        _mockUserManager.Setup(u => u.FindByIdAsync("1"))
            .ReturnsAsync(admin);
        _mockUserManager.Setup(u => u.Users)
            .Returns(users);
        _mockUserManager.Setup(u => u.DeleteAsync(admin))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _adminService.DeleteAsync("1");

        // Assert
        _mockUserManager.Verify(u => u.DeleteAsync(admin), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_AdminNotFound_ThrowsEntityNotFound()
    {
        // Arrange
        _mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
             .Throws<EntityNotFoundException>();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(()
            => _adminService.DeleteAsync("invalid-id"));
    }
    private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        return new Mock<UserManager<TUser>>(
            Mock.Of<IUserStore<TUser>>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!
        );
    }
    #endregion
}