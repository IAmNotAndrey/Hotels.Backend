using Ardalis.GuardClauses;
using AutoMapper;
using Hotels.Application.Dtos;
using Hotels.Application.Dtos.Subobjects;
using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Hotels.Infrastructure.Services;

public class BookingService : IBookingService
{
    private const string ConfigBaseUrl = "BookingService:BaseUrl";
    private const string ConfigHasBookingConflictWithBookingUrl = "BookingService:HasBookingConflictWithBookingUrl";
    private const string ConfigHasBookingConflictWithSubobjectUrl = "BookingService:HasBookingConflictWithSubobjectUrl";

    private readonly IGenericRepo<Booking, Guid> _repo;
    private readonly ISubobjectService _subobjectService;
    private readonly IPaymentService<string> _paymentService;
    private readonly IMapper _mapper;
    private readonly IGenericRepo<Partner, string> _partnerGenericRepo;
    private readonly ISubobjectRepo _subobjectRepo;
    private readonly IConfiguration _configuration;
    private readonly IPartnerRepo _partnerRepo;
    private readonly HttpClient _httpClient;

    public BookingService(
        IGenericRepo<Booking, Guid> repo,
        ISubobjectService subobjectService,
        IPaymentService<string> paymentService,
        IMapper mapper,
        IGenericRepo<Partner, string> partnerGenericRepo,
        ISubobjectRepo subobjectRepo,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IPartnerRepo partnerRepo)
    {
        _repo = repo;
        _subobjectService = subobjectService;
        _paymentService = paymentService;
        _mapper = mapper;
        _partnerGenericRepo = partnerGenericRepo;
        _subobjectRepo = subobjectRepo;
        _configuration = configuration;
        _partnerRepo = partnerRepo;

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(Guard.Against.NullOrWhiteSpace(_configuration[ConfigBaseUrl]));
    }

    public async Task<string> BookAsync(Booking booking)
    {
        // Get the total cost of the reservation for the entire booking period.
        decimal totalPrice = await _subobjectService.CalculateBookingCostAsync(booking.SubobjectId, booking.DateIn, booking.DateOut);

        string paymentId = await _paymentService.CreatePaymentAsync(totalPrice);
        booking.PaymentId = paymentId;

        await _repo.AddAsync(booking);

        return paymentId;
    }

    public async Task<IReadOnlyList<SubobjectDto>> GetBookedSubobjectDtosByPartnerIdAsync(string partnerId, DateOnly dateIn, DateOnly dateOut)
    {
        Partner partner = await _partnerRepo.GetWithSubobjectsAsync(partnerId);
        List<SubobjectDto> subobjectDtos = partner.Subobjects
            .Where(s => HasBookingConflictWithSubobjectAsync(s.Id, dateIn, dateOut).Result)
            .Select(s => _mapper.Map<SubobjectDto>(s))
            .ToList();

        return subobjectDtos;
    }

    public async Task<IReadOnlyList<BookingDto>> GetBookingDtosAsync(Guid subobjectId, DateOnly dateIn, DateOnly dateOut)
    {
        Subobject subobject = await _subobjectRepo.GetSubobjectWithBookingsAsync(subobjectId);
        List<BookingDto> bookingDtos = subobject.Bookings
            .Where(b => HasBookingConflictWithBookingAsync(b.Id, dateIn, dateOut).Result)
            .Select(b => _mapper.Map<BookingDto>(b))
            .ToList();

        return bookingDtos;
    }

    public async Task<bool> HasBookingConflictWithBookingAsync(Guid bookingId, DateOnly startDate, DateOnly endDate)
    {
        string url = Guard.Against.NullOrWhiteSpace(_configuration[ConfigHasBookingConflictWithBookingUrl]);
        url += $"?bookingId={bookingId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        var response = await _httpClient.PostAsync(url, null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new EntityNotFoundException(
                $"{nameof(Booking)} with bookingId '{bookingId}' not found");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Booking service request failed. Status: {response.StatusCode}. Error: {errorContent}");
    }

    public async Task<bool> HasBookingConflictWithSubobjectAsync(Guid subobjectId, DateOnly startDate, DateOnly endDate)
    {
        string url = Guard.Against.NullOrWhiteSpace(_configuration[ConfigHasBookingConflictWithSubobjectUrl]);
        url += $"?subobjectId={subobjectId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        var response = await _httpClient.PostAsync(url, null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new EntityNotFoundException(
                $"{nameof(Subobject)} with SubobjectId '{subobjectId}' not found.");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"Booking service request failed. Status: {response.StatusCode}. Error: {errorContent}");
        throw new NotImplementedException();
    }
}
