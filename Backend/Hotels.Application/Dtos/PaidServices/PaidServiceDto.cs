using Hotels.Application.Dtos.Common;
using Hotels.Domain.Enums;

namespace Hotels.Application.Dtos.PaidServices;

public abstract class PaidServiceDto : ApplicationNamedEntityDto
{
    public PublicationStatus PublicationStatus { get; set; }
    public decimal Price { get; set; }
}
