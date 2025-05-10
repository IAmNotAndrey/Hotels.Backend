using Hotels.Application.Exceptions;
using Hotels.Domain.Entities;

namespace Hotels.Application.Interfaces.Services;

public interface IBookingService
{
    /// <summary>
    ///  Checks if the booking (<paramref name="bookingId"/>) dates conflict with the specified <paramref name="startDate"/> and <paramref name="endDate"/>.
    /// </summary>
    /// <param name="bookingId">Booking Id to check.</param>
    /// <param name="startDate">Start date of the new booking.</param>
    /// <param name="endDate">End date of the new booking.</param>
    /// <returns><see langword="true"/>, if there is a conflict, <see langword="false"/> otherwise.</returns>
    /// <exception cref="EntityNotFoundException">If <see cref="Booking"/> with specified id wasn't found.</exception>
    Task<bool> HasBookingConflictAsync(Guid bookingId, DateOnly startDate, DateOnly endDate);
}
