using Hotels.Application.Dtos.Reviews;
using Hotels.Application.Dtos.Users;

namespace Hotels.Application.Dtos;

public class TouristDto : ApplicationUserDto
{
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    // ===

    public virtual ICollection<BookingDto> Bookings { get; set; } = [];
    public virtual ICollection<PartnerReviewDto> Reviews { get; set; } = [];
}
