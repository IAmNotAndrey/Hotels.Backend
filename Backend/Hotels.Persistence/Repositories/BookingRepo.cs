using Ardalis.GuardClauses;
using Hotels.Application.Exceptions;
using Hotels.Domain.Entities;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Hotels.Persistence.Repositories;

public class BookingRepo : IBookingRepo
{
    private const string ConfigGetByPaymentIdUrl = "BookingService:GetByPaymentIdUrl";

    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string _getByPaymentIdUrl;

    public BookingRepo(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        string getByPaymentIdUrl = Guard.Against.NullOrWhiteSpace(
            configuration[ConfigGetByPaymentIdUrl],
            nameof(getByPaymentIdUrl));
        _getByPaymentIdUrl = getByPaymentIdUrl!;

        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Booking> GetByPaymentIdAsync(string paymentId)
    {
        string url = _getByPaymentIdUrl + paymentId;

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Booking>()
                ?? throw new InvalidOperationException($"Failed to deserialize {nameof(Booking)}");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new EntityNotFoundException(
                $"{nameof(Booking)} with PaymentId '{paymentId}' not found.");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Booking service request failed. Status: {response.StatusCode}. Error: {errorContent}");
    }
}
