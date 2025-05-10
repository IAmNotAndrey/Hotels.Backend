namespace Hotels.Application.Interfaces.Services;

public interface IToiletService
{
    Task LinkAsync(Guid subobjectId, Guid toiletId);
    Task UnlinkAsync(Guid subobjectId, Guid toiletId);
}
