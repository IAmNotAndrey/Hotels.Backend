using Hotels.Domain.Entities.Subobjects;

namespace Hotels.Domain.Entities.EntityTypes;

public class RoomType : SubobjectType
{
    public ICollection<Room> Rooms { get; set; } = [];
}
