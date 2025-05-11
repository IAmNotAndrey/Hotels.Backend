using Hotels.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class SmsAeroSenderServiceTests
{
    private const string Login = "test-login";
    private const string ApiKey = "test-apiKey";

    private readonly Dictionary<string, string> _validConfig = new()
    {
        { "SmsAero:Login", Login },
        { "SmsAero:ApiKey", ApiKey }
    };

    private readonly Mock<ILogger<SmsAeroSenderService>> _loggerMock = new();

    private static IConfiguration CreateConfiguration(Dictionary<string, string?> config)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }

    [Fact]
    public void Constructor_WithValidConfig_InitializesSuccessfully()
    {
        // Arrange
        var config = CreateConfiguration(_validConfig!);

        // Act
        var exception = Record.Exception(() => new SmsAeroSenderService(_loggerMock.Object, config));

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("SmsAero:Login")]
    [InlineData("SmsAero:ApiKey")]
    public void Constructor_MissingRequiredKey_ThrowsException(string missingKey)
    {
        // Arrange
        var invalidConfig = new Dictionary<string, string>(_validConfig);
        invalidConfig.Remove(missingKey);

        var config = CreateConfiguration(invalidConfig!);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new SmsAeroSenderService(_loggerMock.Object, config));
    }
}
