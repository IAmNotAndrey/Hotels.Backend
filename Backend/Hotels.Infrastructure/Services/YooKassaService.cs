using Ardalis.GuardClauses;
using Hotels.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Yandex.Checkout.V3;

namespace Hotels.Infrastructure.Services;

public class YooKassaService : IPaymentService<string>
{
    private const string ConfigKeyShopId = "YooKassa:ShopId";
    private const string ConfigKeySecretKey = "YooKassa:SecretKey";
    private const string ConfigKeyReturnUrl = "YooKassa:ReturnUrl";

    private readonly AsyncClient _client;
    private readonly string _returnUrl;
    private readonly ILogger<YooKassaService> _logger;

    public YooKassaService(ILogger<YooKassaService> logger, IConfiguration configuration)
    {
        _logger = logger;

        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyShopId]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeySecretKey]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyReturnUrl]);

        // Получаем настройки из конфигурации
        var shopId = configuration[ConfigKeyShopId];
        var secretKey = configuration[ConfigKeySecretKey];
        _returnUrl = configuration[ConfigKeyReturnUrl]!;

        _client = new Client(shopId: shopId, secretKey: secretKey).MakeAsync();
    }

    public async Task CapturePaymentAsync(string id)
    {
        await _client.CapturePaymentAsync(id);
    }

    public async Task<string> CreatePaymentAsync(decimal amount)
    {
        NewPayment newPayment = new()
        {
            Amount = new Amount { Value = amount, Currency = "RUB" },
            Confirmation = new Confirmation
            {
                Type = ConfirmationType.Redirect,
                ReturnUrl = _returnUrl,
            },
            Capture = true,
        };
        Payment payment = await _client.CreatePaymentAsync(newPayment);

        _logger.LogInformation("New YooKassa payment has been registered with id: {Id}.", payment.Id);
        return payment.Id;
    }
}
