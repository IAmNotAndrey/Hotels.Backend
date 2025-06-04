using Hotels.Application.Dtos;
using Hotels.Application.Dtos.Subobjects;
using Hotels.Application.Exceptions;
using Hotels.Domain.Entities;

namespace Hotels.Application.Interfaces.Services;

public interface IBookingService
{
    Task<string> BookAsync(Booking booking);

    Task<IReadOnlyList<SubobjectDto>> GetBookedSubobjectDtosByPartnerIdAsync(string partnerId, DateOnly dateIn, DateOnly dateOut);

    /// <returns>
    /// Все 'Bookings', у 'Partner.Subobjects' где есть пересечения с интервалом (dateIn, dateOut).
    /// </returns>
    Task<IReadOnlyList<BookingDto>> GetBookingDtosAsync(Guid subobjectId, DateOnly dateIn, DateOnly dateOut);

    /// <summary>
    ///  Checks if the booking (<paramref name="bookingId"/>) dates conflict with the specified <paramref name="startDate"/> and <paramref name="endDate"/>.
    /// </summary>
    /// <param name="bookingId">Booking Id to check.</param>
    /// <param name="startDate">Start date of the new booking.</param>
    /// <param name="endDate">End date of the new booking.</param>
    /// <returns><see langword="true"/>, if there is a conflict, <see langword="false"/> otherwise.</returns>
    /// <exception cref="EntityNotFoundException">If <see cref="Booking"/> with specified id wasn't found.</exception>
    Task<bool> HasBookingConflictWithBookingAsync(Guid bookingId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Проверяет, есть ли конфликт бронирования для указанного подобъекта на основе заданного интервала дат.
    /// </summary>
    /// <param name="subobjectId">Идентификатор подобъекта.</param>
    /// <param name="startDate">Дата начала нового бронирования.</param>
    /// <param name="endDate">Дата окончания нового бронирования.</param>
    /// <returns>Возвращает <see langword="true"/>, если есть конфликт бронирования; в противном случае <see langword="false"/>.</returns>
    /// <exception cref="EntityNotFoundException">Выбрасывается, если `Subobject` не найден.</exception>
    Task<bool> HasBookingConflictWithSubobjectAsync(Guid subobjectId, DateOnly startDate, DateOnly endDate);
}
