using Hotels.Domain.Enums;
using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.PaidServices;

public abstract class PaidServiceDtoB : ApplicationBaseEntityDtoB
{
    [Required] public PublicationStatus PublicationStatus { get; set; }
    [Required, Range(0, double.MaxValue)] public decimal Price { get; set; }
}
