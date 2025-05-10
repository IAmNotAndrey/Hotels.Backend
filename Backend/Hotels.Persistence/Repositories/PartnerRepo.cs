using AutoMapper;
using Hotels.Application.Dtos.Users;
using Hotels.Application.Exceptions;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;
using Hotels.Presentation.DtoBs.Users;
using Hotels.Presentation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Persistence.Repositories;

public class PartnerRepo : IPartnerRepo
{
    private readonly ApplicationContext _db;
    private readonly IGenericRepo<Partner, string> _repo;
    private readonly IMapper _mapper;

    public PartnerRepo(ApplicationContext db, IGenericRepo<Partner, string> repo, IMapper mapper)
    {
        _db = db;
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PartnerDto> GetDtoIncludedAsync(string id)
    {
        Partner partner = await GetIncludedAsync(id);
        PartnerDto dto = _mapper.Map<PartnerDto>(partner);
        return dto;
    }

    public async Task<Partner> GetIncludedAsync(string id)
    {
        Partner partner = await _db.Partners
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Comforts)
            .Include(p => p.Contacts)
            .Include(p => p.Feeds)
            .Include(p => p.ImageLinks)
            .Include(p => p.Nearbies)
            .Include(p => p.Subobjects)
                .ThenInclude(so => so.WeekRate)
            .Include(p => p.Reviews)
                .ThenInclude(r => r.ImageLinks)
            .Include(p => p.City)
            .Include(p => p.Type)
            .FirstAsync(p => p.Id == id);
        return partner;
    }

    public async Task<IEnumerable<PartnerDto>> GetAllDtosIncludedIncludeAsync()
    {
        IEnumerable<string> ids = _repo.Entities.Select(e => e.Id);
        IEnumerable<Partner> partners = await GetIncludedAsync(ids);
        IEnumerable<PartnerDto> dtos = GetMapped(partners);
        return dtos;
    }
    private async Task<IEnumerable<Partner>> GetIncludedAsync(IEnumerable<string> ids)
    {
        List<Partner> partners = [];
        foreach (var id in ids)
        {
            var p = await GetIncludedAsync(id);
            partners.Add(p);
        }
        return partners;
    }

    private async Task<IEnumerable<PartnerDto>> GetDtosIncludedAsync(IEnumerable<string> ids)
    {
        IEnumerable<Partner> partners = await GetIncludedAsync(ids);
        IEnumerable<PartnerDto> dtos = GetMapped(partners);
        return dtos;
    }

    public async Task<IEnumerable<PartnerDto>> GetDtosIncludedByFilterAsync(IFilterModel<Partner> filter)
    {
        IEnumerable<Partner> partners = await _repo.GetAllAsync(filter);
        IEnumerable<string> ids = partners.Select(e => e.Id);
        IEnumerable<Partner> included = await GetIncludedAsync(ids);
        IEnumerable<PartnerDto> dtos = GetMapped(included);
        return dtos;
    }

    public async Task UpdateAsync(string id, PartnerDtoB dtoB)
    {
        Partner partner = _mapper.Map<Partner>(dtoB);
        partner.Id = id;
        partner.AccountStatus = AccountStatus.OnModeration;
        await _repo.UpdateAsync(partner);
    }



    public async Task<IEnumerable<PartnerDto>> GetDtosIncludedAdvertisingAsync(Guid countrySubjectId)
    {
        IEnumerable<string> ids = _db.Partners
            .AsNoTracking()
            .Include(e => e.City)
            .Where(p => p.IsPublished
                     && p.IsAdvertised
                     && p.City != null
                     && p.City.CountrySubjectId == countrySubjectId)
            .Select(e => e.Id);
        IEnumerable<PartnerDto> dtos = await GetDtosIncludedAsync(ids);
        return dtos;
    }

    public async Task<IEnumerable<PartnerDto>> GetDtosIncludedPromoSeriesAsync(Guid countrySubjectId)
    {
        IEnumerable<string> ids = _db.Partners
            .AsNoTracking()
            .Include(e => e.City)
            .Where(p => p.IsPublished
                     && p.IsPromoSeries
                     && p.City != null
                     && p.City.CountrySubjectId == countrySubjectId)
            .Select(e => e.Id);
        IEnumerable<PartnerDto> dtos = await GetDtosIncludedAsync(ids);
        return dtos;
    }


    private IEnumerable<PartnerDto> GetMapped(IEnumerable<Partner> partners)
    {
        return partners.Select(e => _mapper.Map<PartnerDto>(e));
    }
}
