using Hotels.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class EmailSenderServiceTests
{
    private const string FromEmail = "test@example.com";
    private const string FromPassword = "test-password";
    private const string FromName = "test-from-name";
    private const string Host = "smtp.example.com";
    private const int Port = 587;
    private const bool UseSsl = true;

    private readonly Dictionary<string, string> _validConfig = new()
    {
        { "EmailSender:FromEmail", FromEmail },
        { "EmailSender:FromPassword", FromPassword },
        { "EmailSender:FromName", FromName },
        { "EmailSender:Host", Host },
        { "EmailSender:Port", Port.ToString() },
        { "EmailSender:UseSsl", UseSsl.ToString() }
    };

    private readonly Mock<ILogger<EmailSenderService>> _loggerMock = new();

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
        var loggerMock = new Mock<ILogger<EmailSenderService>>();

        // Act
        var exception = Record.Exception(() => new EmailSenderService(loggerMock.Object, config));

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("EmailSender:FromEmail")]
    [InlineData("EmailSender:FromPassword")]
    [InlineData("EmailSender:FromName")]
    [InlineData("EmailSender:Host")]
    [InlineData("EmailSender:Port")]
    [InlineData("EmailSender:UseSsl")]
    public void Constructor_MissingRequiredKey_ThrowsException(string missingKey)
    {
        // Arrange
        var invalidConfig = new Dictionary<string, string>(_validConfig);
        invalidConfig.Remove(missingKey);

        var config = CreateConfiguration(invalidConfig);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new EmailSenderService(_loggerMock.Object, config));
    }

    [Theory]
    [InlineData("EmailSender:Port", "invalid-port")]
    [InlineData("EmailSender:UseSsl", "invalid-bool")]
    public void Constructor_InvalidFormatValue_ThrowsFormatException(string key, string invalidValue)
    {
        // Arrange
        var invalidConfig = new Dictionary<string, string>(_validConfig)
        {
            [key] = invalidValue
        };

        var config = CreateConfiguration(invalidConfig);

        // Act & Assert
        Assert.Throws<FormatException>(() =>
            new EmailSenderService(_loggerMock.Object, config));
    }

    [Theory]
    [InlineData("EmailSender:FromEmail", null)]
    [InlineData("EmailSender:FromPassword", null)]
    [InlineData("EmailSender:FromName", null)]
    [InlineData("EmailSender:Host", null)]
    [InlineData("EmailSender:Port", null)]
    [InlineData("EmailSender:UseSsl", null)]
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
            new EmailSenderService(_loggerMock.Object, config));
    }

    [Theory]
    [InlineData("EmailSender:FromPassword", "")]
    [InlineData("EmailSender:FromPassword", " ")]
    [InlineData("EmailSender:FromName", "")]
    [InlineData("EmailSender:FromName", " ")]
    [InlineData("EmailSender:Host", "")]
    [InlineData("EmailSender:Host", " ")]
    [InlineData("EmailSender:Port", "")]
    [InlineData("EmailSender:Port", " ")]
    [InlineData("EmailSender:UseSsl", "")]
    [InlineData("EmailSender:UseSsl", " ")]
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
            new EmailSenderService(_loggerMock.Object, config));
    }
}
