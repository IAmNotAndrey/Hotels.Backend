using Ardalis.GuardClauses;
using Hotels.Application.Dtos.Reviews;
using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Reviews;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Hotels.Persistence.Repositories;

public class ReviewRepo : IReviewRepo
{
    private const string ConfigBaseUrl = "PartnerReviewsService:BaseUrl";
    private const string ConfigGetDtosIncludedUrl = "PartnerReviewsService:GetDtosIncludedUrl";
    private const string ConfigGetDtoIncludedUrl = "PartnerReviewsService:GetDtoIncludedUrl";
    private const string ConfigGetDtosIncludedByPartnerUrl = "PartnerReviewsService:GetDtosIncludedByPartnerUrl";
    private const string ConfigGetDtosIncludedByTouristUrl = "PartnerReviewsService:GetDtosIncludedByTouristUrl";

    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ReviewRepo(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new(Guard.Against.NullOrWhiteSpace(_configuration[ConfigBaseUrl]));
    }

    public async Task<PartnerReviewDto> GetDtoIncludedAsync(Guid id)
    {
        string url = Guard.Against.NullOrWhiteSpace(_configuration[ConfigGetDtoIncludedUrl]);
        url += id.ToString();

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PartnerReviewDto>()
                ?? throw new InvalidOperationException($"Failed to deserialize {nameof(PartnerReviewDto)}");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new EntityNotFoundException(
                $"{nameof(PartnerReview)} with Id '{id}' not found.");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Review service request failed. Status: {response.StatusCode}. Error: {errorContent}");
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedAsync()
    {
        string url = Guard.Against.NullOrWhiteSpace(_configuration[ConfigGetDtosIncludedUrl]);

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>()
                ?? throw new InvalidOperationException($"Failed to deserialize {nameof(PartnerReviewDto)}s");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Review service request failed. Status: {response.StatusCode}. Error: {errorContent}");
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByPartnerAsync(string partnerId)
    {
        string url = Guard.Against.NullOrWhiteSpace(_configuration[ConfigGetDtosIncludedByPartnerUrl]);
        url += partnerId;

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>()
                ?? throw new InvalidOperationException($"Failed to deserialize {nameof(PartnerReviewDto)}s");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Review service request failed. Status: {response.StatusCode}. Error: {errorContent}");
    }

    public async Task<IEnumerable<PartnerReviewDto>> GetDtosIncludedByTouristAsync(string touristId)
    {
        string url = Guard.Against.NullOrWhiteSpace(_configuration[ConfigGetDtosIncludedByTouristUrl]);
        url += touristId;

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>()
                ?? throw new InvalidOperationException($"Failed to deserialize {nameof(PartnerReviewDto)}s");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Review service request failed. Status: {response.StatusCode}. Error: {errorContent}");
    }
}
