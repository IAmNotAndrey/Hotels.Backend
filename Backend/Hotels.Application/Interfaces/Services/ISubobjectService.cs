using Hotels.Application.Exceptions;

namespace Hotels.Application.Interfaces.Services;

public interface ISubobjectService
{
    /// <summary>
    /// Вычисляет стоимость бронирования для указанного подобъекта на основе заданного интервала дат.
    /// </summary>
    /// <param name="subobjectId">Идентификатор подобъекта.</param>
    /// <param name="dateIn">Дата начала бронирования.</param>
    /// <param name="dateOut">Дата окончания бронирования.</param>
    /// <returns>Общая стоимость бронирования.</returns>
    /// <exception cref="EntityNotFoundException">Выбрасывается, если `Subobject` или `Subobject.WeekRate` не найдены.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если для какого-либо дня недели не задана ставка.</exception>
    Task<decimal> CalculateBookingCostAsync(Guid subobjectId, DateOnly dateIn, DateOnly dateOut);
}
