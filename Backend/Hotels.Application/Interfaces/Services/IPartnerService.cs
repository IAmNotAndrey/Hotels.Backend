using Hotels.Domain.Entities.Users;

namespace Hotels.Application.Interfaces.Services;

public interface IPartnerService
{
    Task SetToModerateAsync(string id);
    /// <summary>
    /// Проверяет, заполнен ли Partner необходимыми данными для отправки на модерацию
    /// </summary>
    bool IsValidForModeration(Partner partner, out List<string> validationErrors);
}
