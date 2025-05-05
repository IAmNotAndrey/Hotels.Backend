namespace Hotels.Persistence.Interfaces.Repositories;

public interface IToiletRepo
{
    Task LinkAsync(Guid subobjectId, Guid toiletId);
    Task UnlinkAsync(Guid subobjectId, Guid toiletId);
}
