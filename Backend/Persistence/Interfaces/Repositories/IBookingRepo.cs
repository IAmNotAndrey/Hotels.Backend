using Hotels.Application.Exceptions;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IBookingRepo
{
    /// <summary>
    /// Проверяет, есть ли конфликт бронирования с указанными датами.
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования, которое нужно проверить.</param>
    /// <param name="startDate">Дата начала нового бронирования.</param>
    /// <param name="endDate">Дата окончания нового бронирования.</param>
    /// <returns>Возвращает true, если конфликт существует, иначе false.</returns>
    /// <exception cref="EntityNotFoundException">Если <see cref="Booking"/> с указанным идентификатором не найдено.</exception>
    Task<bool> HasBookingConflictAsync(Guid bookingId, DateOnly startDate, DateOnly endDate);
}
