using Hotels.Application.Dtos.Types.SubobjectTypes;

namespace Hotels.Application.Dtos.Subobjects;

public class HousingDto : SubobjectDto
{
    public int BedroomCount { get; set; }

    // ===

    public HousingTypeDto Type { get; set; } = null!;
}
