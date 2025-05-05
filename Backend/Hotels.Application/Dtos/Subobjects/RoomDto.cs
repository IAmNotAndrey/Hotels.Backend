using Hotels.Application.Dtos.Types.SubobjectTypes;

namespace Hotels.Application.Dtos.Subobjects;

public class RoomDto : SubobjectDto
{
    public RoomTypeDto Type { get; set; } = null!;
}
