using Hotels.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hotels.Infrastructure.Services;

public class StaticFilesService : IStaticFilesService
{
    private readonly ILogger<StaticFilesService> _logger;

    public StaticFilesService(ILogger<StaticFilesService> logger)
    {
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);

        var filePath = Path.Combine(directoryPath, Path.GetFileName(file.FileName));
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        _logger.LogInformation("File '{FileName}' has been saved at '{FilePath}'.", file.FileName, filePath);
        return filePath;
    }

    public bool Remove(string filePath)
    {
        if (!File.Exists(filePath))
        {
            _logger.LogInformation("File at the path '{FilePath}' was not found to be deleted.", filePath);
            return false;
        }
        File.Delete(filePath);

        _logger.LogInformation("File at the path '{FilePath}' has been deleted.", filePath);
        return true;
    }
}
