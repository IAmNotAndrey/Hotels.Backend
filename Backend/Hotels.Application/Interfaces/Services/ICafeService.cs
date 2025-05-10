using Microsoft.AspNetCore.Http;

namespace Hotels.Application.Interfaces.Services;

public interface ICafeService
{
    Task SaveMenuFileAsync(Guid id, IFormFile menuFile);
}
