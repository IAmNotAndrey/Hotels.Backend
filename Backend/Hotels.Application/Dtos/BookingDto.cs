using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos;

public class BookingDto : ApplicationBaseEntityDto
{
    public DateTime CreatedAt { get; set; }
    public DateOnly DateIn { get; set; }
    public required DateOnly DateOut { get; set; }
    public string PaymentId { get; set; } = null!;
    public decimal? AdditionalPrice { get; set; }

    // ===

    public Guid SubobjectId { get; set; }
    public string TouristId { get; set; } = null!;
}
