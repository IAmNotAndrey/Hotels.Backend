using Hotels.Bookings.Infrastructure.Services;
using Hotels.Bookings.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hotels.BookingsService.Controllers;

[ApiController]
[Route("api/internal/v1/bookings/[action]")]
public class BookingInternalController(IBookingRepo bookingRepo,
                                       BookingService bookingService) : ControllerBase
{
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetByPaymentId(string paymentId)
    {
        return Ok(await bookingRepo.GetByPaymentIdAsync(paymentId));
    }

    [HttpPost]
    public async Task<IActionResult> HasBookingConflictWithBooking(
        [Required] Guid bookingId,
        [Required] DateOnly startDate,
        [Required] DateOnly endDate)
    {
        bool result = await bookingService.HasBookingConflictWithBookingAsync(bookingId, startDate, endDate);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> HasBookingConflictWithSubobject(
    [Required] Guid subobjectId,
    [Required] DateOnly startDate,
    [Required] DateOnly endDate)
    {
        bool result = await bookingService.HasBookingConflictWithSubobjectAsync(subobjectId, startDate, endDate);
        return Ok(result);
    }
}
