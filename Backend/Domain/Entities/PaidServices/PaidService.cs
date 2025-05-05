using Hotels.Domain.Common;
using Hotels.Domain.Enums;

namespace Hotels.Domain.Entities.PaidServices;

public abstract class PaidService : ApplicationNamedEntity
{
    /// <summary>
    /// Является ли сервис активным
    /// </summary>
    public PublicationStatus PublicationStatus { get; set; } = PublicationStatus.Published;
    public decimal Price { get; set; }
}
