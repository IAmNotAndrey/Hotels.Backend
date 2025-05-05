using Microsoft.AspNetCore.Http;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface ICafeRepo
{
    Task SaveMenuFileAsync(Guid id, IFormFile menuFile);
}
