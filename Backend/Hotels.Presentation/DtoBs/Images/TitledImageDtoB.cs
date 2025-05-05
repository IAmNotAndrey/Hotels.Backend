using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Images;

public class TitledImageDtoB
{
    [Required] public IFormFile Image { get; set; } = null!;
    [Required] public bool IsTitle { get; set; } = false;
}
