using Ardalis.GuardClauses;
using Hotels.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmsAero;

namespace Hotels.Infrastructure.Services;

public class SmsAeroSenderService : ISmsSender
{
    private const string Login = "SmsAero:Login";
    private const string ApiKey = "SmsAero:ApiKey";

    private readonly SmsAeroClient _client;
    private readonly ILogger<SmsAeroSenderService> _logger;

    public SmsAeroSenderService(ILogger<SmsAeroSenderService> logger, IConfiguration configuration)
    {
        _logger = logger;

        Guard.Against.NullOrWhiteSpace(configuration[Login]);
        Guard.Against.NullOrWhiteSpace(configuration[ApiKey]);

        string login = configuration[Login]!;
        string apiKey = configuration[ApiKey]!;

        _client = new(login, apiKey);
    }

    public async Task SendSmsAsync(string number, string text)
    {
        await Task.Run(() =>
        {
            _client.SmsSend(text: text, number: number);
        });
        _logger.LogInformation("The message was successfully sent to the number: {number}", number);
    }
}
