using Hotels.Application.Exceptions;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities.Users;
using Hotels.Domain.Enums;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Interfaces.Repositories;

namespace Hotels.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly ApplicationContext _db;
    private readonly IGenericRepo<Partner, string> _repo;

    public PartnerService(ApplicationContext db, IGenericRepo<Partner, string> repo)
    {
        _db = db;
        _repo = repo;
    }

    public async Task SetToModerateAsync(string id)
    {
        Partner partner = await _repo.GetByIdOrDefaultAsync(id, asNoTracking: false)
            ?? throw new EntityNotFoundException($"{nameof(Partner)} wasn't found");
        partner.AccountStatus = AccountStatus.OnModeration;
        await _db.SaveChangesAsync();
    }

    public bool IsValidForModeration(Partner partner, out List<string> validationErrors)
    {
        // fixme неправильная валидация
        validationErrors = [];

        // Проверка необходимых полей
        if (string.IsNullOrWhiteSpace(partner.Name))
        {
            validationErrors.Add($"{nameof(partner.Name)} is required.");
        }
        if (partner.TypeId == null)
        {
            validationErrors.Add($"{nameof(partner.TypeId)} is required.");
        }
        if (partner.CityId == null)
        {
            validationErrors.Add($"{nameof(partner.CityId)} is required.");
        }
        if (partner.TypeId == null)
        {
            validationErrors.Add($"At least one of {nameof(partner.Contacts)} is required.");
        }
        if (partner.Contacts.Count == 0)
        {
            validationErrors.Add($"At least one of {nameof(partner.Contacts)} is required.");
        }

        return validationErrors.Count == 0;
    }
}
