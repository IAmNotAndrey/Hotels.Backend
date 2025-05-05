using AutoMapper;
using Hotels.Application.Dtos.Subobjects;
using Hotels.Domain.Entities.Subobjects;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class RoomRepo : IRoomRepo
{
    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;

    public RoomRepo(ApplicationContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDto>> GetDtosIncludedByPartnerAsync(string partnerId)
    {
        Guid[] ids = await _db.Rooms
            .AsNoTracking()
            .Where(e => e.PartnerId == partnerId)
            .Select(e => e.Id)
            .ToArrayAsync();
        IEnumerable<RoomDto> dtos = (await GetInclude(ids))!
            .Select(e => _mapper.Map<RoomDto>(e));
        return dtos;
    }

    private async Task<Room?> GetInclude(Guid id)
    {
        Room? room = await _db.Rooms
            .AsNoTracking()
            .AsSplitQuery()
            .Include(h => h.Comforts)
            .Include(h => h.WeekRate)
            .Include(h => h.Feeds)
            .Include(h => h.Bookings)
            .Include(h => h.ImageLinks)
            .Include(h => h.Partner)
            .Include(h => h.Toilets)
            .Include(h => h.Bathrooms)
            .Include(h => h.Type)
            .FirstOrDefaultAsync(p => p.Id == id);
        return room;
    }

    private async Task<IEnumerable<Room?>> GetInclude(IEnumerable<Guid> ids)
    {
        List<Room?> rooms = [];
        foreach (var id in ids)
        {
            var p = await GetInclude(id);
            rooms.Add(p);
        }
        return rooms;
    }
}
