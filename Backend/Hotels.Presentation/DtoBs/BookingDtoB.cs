using Hotels.Presentation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs;

public class BookingDtoB
{
    /// <summary>
    /// Дата заезда
    /// </summary>
    [Required] public required DateOnly DateIn { get; init; }

    /// <summary>
    /// Дата выезда
    /// </summary>
    [Required, GreaterOrEqualThan(nameof(DateIn))]
    public required DateOnly DateOut { get; init; }

    [Range(0, double.MaxValue)]
    public decimal? AdditionalPrice { get; set; } = null; // fixme : не совсем понятно, как и при каких обстоятельствах использовать

    // ===

    [Required] public required Guid SubobjectId { get; set; }
    [Required] public required string TouristId { get; set; }
}
