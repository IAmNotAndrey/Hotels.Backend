using Hotels.Domain.Entities.Reviews;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotels.Domain.Entities.Users;

public class Tourist : ApplicationUser
{
    [NotMapped] public override IdentityRole Role => new(nameof(Tourist));

    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<PartnerReview> ObjectReviews { get; set; } = [];

    public Tourist()
    {
        Name = "Турист"; // FIXME: bad implementation - it's better to use service.
    }
}
