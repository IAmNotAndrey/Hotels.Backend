using Microsoft.AspNetCore.Http;

namespace Hotels.Application.Interfaces.Services;

public interface IStaticFilesService
{
    Task<string> SaveFileAsync(IFormFile file, string directoryPath);

    /// <summary>
    /// Удаляет статический файл по <paramref name="filePath"/>
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns><see langword="true"/> при успешном удалении, <see langword="false"/> иначе</returns>
    bool Remove(string filePath);
}
