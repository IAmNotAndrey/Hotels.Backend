using Hotels.Domain.Common;
using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Domain.Entities.Users;

namespace Hotels.Domain.Entities;

public class Booking : ApplicationBaseEntity, ICreatedAt
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;


    // TODO : сделать ограничения на прошедшие даты

    /// <summary>
    /// Дата заезда
    /// </summary>
    public required DateOnly DateIn { get; init; }

    /// <summary>
    /// Дата выезда
    /// </summary>
    public required DateOnly DateOut { get; init; }

    /// <summary>
    /// Id привязанного к брони платежа YooKassa
    /// </summary>
    public string PaymentId { get; set; } = null!;

    public decimal? AdditionalPrice { get; set; } = null; // fixme : не совсем понятно, как и при каких обстоятельствах использовать

    // ===

    public Guid SubobjectId { get; set; }
    public Subobject Subobject { get; set; } = null!;

    public string TouristId { get; set; } = null!;
    public Tourist Tourist { get; set; } = null!;
}
