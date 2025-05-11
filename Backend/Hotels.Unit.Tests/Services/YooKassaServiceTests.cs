using Hotels.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class YooKassaServiceTests
{
    private const string ShopId = "test_shop_id";
    private const string SecretKey = "test_secret_key";
    private const string ReturnUrl = "https://example.com/return";

    private readonly Dictionary<string, string> _validConfig = new()
    {
        { "YooKassa:ShopId", ShopId },
        { "YooKassa:SecretKey", SecretKey },
        { "YooKassa:ReturnUrl", ReturnUrl }
    };

    private readonly Mock<ILogger<YooKassaService>> _loggerMock = new();

    private static IConfiguration CreateConfiguration(Dictionary<string, string> config)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(config!)
            .Build();
    }

    [Fact]
    public void Constructor_WithValidConfig_InitializesSuccessfully()
    {
        // Arrange
        var config = CreateConfiguration(_validConfig);

        // Act
        var exception = Record.Exception(() => new YooKassaService(_loggerMock.Object, config));

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("YooKassa:ShopId")]
    [InlineData("YooKassa:SecretKey")]
    [InlineData("YooKassa:ReturnUrl")]
    public void Constructor_MissingRequiredKey_ThrowsException(string missingKey)
    {
        // Arrange
        var invalidConfig = new Dictionary<string, string>(_validConfig);
        invalidConfig.Remove(missingKey);

        var config = CreateConfiguration(invalidConfig);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new YooKassaService(_loggerMock.Object, config));
    }

    [Theory]
    [InlineData("YooKassa:ShopId", null)]
    [InlineData("YooKassa:SecretKey", null)]
    [InlineData("YooKassa:ReturnUrl", null)]
    public void Constructor_EmptyOrWhitespaceValue_ThrowsArgumentNullException(string key, string invalidValue)
    {
        // Arrange
        var invalidConfig = new Dictionary<string, string>(_validConfig)
        {
            [key] = invalidValue
        };

        var config = CreateConfiguration(invalidConfig);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new YooKassaService(_loggerMock.Object, config));
    }

    [Theory]
    [InlineData("YooKassa:ShopId", "")]
    [InlineData("YooKassa:ShopId", " ")]
    [InlineData("YooKassa:SecretKey", "")]
    [InlineData("YooKassa:SecretKey", " ")]
    [InlineData("YooKassa:ReturnUrl", "")]
    [InlineData("YooKassa:ReturnUrl", " ")]
    public void Constructor_EmptyOrWhitespaceValue_ThrowsArgumentException(string key, string invalidValue)
    {
        // Arrange
        var invalidConfig = new Dictionary<string, string>(_validConfig)
        {
            [key] = invalidValue
        };

        var config = CreateConfiguration(invalidConfig);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new YooKassaService(_loggerMock.Object, config));
    }
}
