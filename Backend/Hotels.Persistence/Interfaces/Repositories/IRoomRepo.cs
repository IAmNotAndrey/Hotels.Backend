using Hotels.Application.Dtos.Subobjects;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IRoomRepo
{
    Task<IEnumerable<RoomDto>> GetDtosIncludedByPartnerAsync(string partnerId);
}
