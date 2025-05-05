using Hotels.Domain.Common.Interfaces;
using Hotels.Domain.Entities.EntityTypes;

namespace Hotels.Domain.Entities.Subobjects;

public class Room : Subobject, IHasType<RoomType>
{
    public override string TypeName { get; init; } = nameof(Room);
    // addme? Сезон только для ?? housing ??

    public required Guid? TypeId { get; set; }
    public RoomType? Type { get; set; } = null!;
}
