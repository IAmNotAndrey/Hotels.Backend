using Ardalis.GuardClauses;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Domain.Entities.Users;
using Hotels.Persistence.Contexts;
using Hotels.Persistence.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hotels.Infrastructure.Services;

public class NearbyService : INearbyService
{
    private const string ConfigKeyStaticFilesDirPath = "StaticFiles:DirectoryPath";
    private const string ConfigKeyImageDirPath = "StaticFiles:NearbyImage:DirectoryPath";
    private const string ConfigKeyImageSupportedExtensions = "StaticFiles:NearbyImage:SupportedExtensions";

    private readonly ApplicationContext _db;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<NearbyRepo> _logger;
    private readonly IStaticFilesService _staticFilesService;

    private readonly string _staticFilesDirPath;
    private readonly string _imageDirPath;
    private readonly HashSet<string> _imageSupportedExtensions;

    public NearbyService(ApplicationContext db, 
                         IWebHostEnvironment environment, 
                         ILogger<NearbyRepo> logger, 
                         IStaticFilesService staticFilesService, 
                         IConfiguration configuration)
    {
        _db = db;
        _environment = environment;
        _logger = logger;
        _staticFilesService = staticFilesService;

        // Get data from config
        var staticFilesDirPath = configuration[ConfigKeyStaticFilesDirPath];
        Guard.Against.NullOrWhiteSpace(staticFilesDirPath, message: $"The '{nameof(staticFilesDirPath)}' wasn't found by the path '{ConfigKeyStaticFilesDirPath}'.");
        _staticFilesDirPath = staticFilesDirPath;

        var imageDirPath = configuration[ConfigKeyImageDirPath];
        Guard.Against.NullOrWhiteSpace(imageDirPath, message: $"The '{imageDirPath}' wasn't found by the path '{ConfigKeyImageDirPath}'.");
        _imageDirPath = imageDirPath;
        // Get extensions from config.
        var imageSupportedExtensions = configuration.GetSection(ConfigKeyImageSupportedExtensions);
        Guard.Against.Null(imageSupportedExtensions, nameof(imageSupportedExtensions), message: $"The '{imageSupportedExtensions}' wasn't found by the path '{ConfigKeyImageSupportedExtensions}'.");
        _imageSupportedExtensions = imageSupportedExtensions.Get<List<string>>()?.ToHashSet()
            ?? throw new InvalidOperationException("SupportedExtensions section is missing or invalid.");
    }

    public async Task SetImageLinkAsync(Guid id, IFormFile file)
    {
        var nearby = await _db.Nearbies
            .Include(e => e.ImageLink)
            .FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new ArgumentException($"{nameof(Nearby)} wasn't found by id '{id}'", nameof(id));

        // Check whether `file` has suitable extension.
        if (!IsSupportedFileType(file.FileName))
        {
            _logger.LogError("Menu file extension not supported: {FileName}", file.FileName);
            throw new InvalidOperationException($"Menu file extension not supported: {file.FileName}");
        }
        // Create full directory path to save file.
        string dirPath = Path.Combine(_environment.WebRootPath, _staticFilesDirPath, _imageDirPath, nearby.ToString());
        // Get full path of the saved file.
        var fullPath = await _staticFilesService.SaveFileAsync(file, dirPath);

        // Set new menu link deleting existing.
        NearbyImageLink menuLink = new() { NearbyId = nearby.Id, Uri = fullPath };
        _db.NearbyImageLinks.Add(menuLink);

        await _db.SaveChangesAsync();
    }

    public async Task LinkAsync(string partnerId, Guid nearbyId)
    {
        Partner partner = await _db.Partners
            .Include(e => e.Nearbies)
            .FirstAsync(e => e.Id == partnerId);
        Nearby nearby = await _db.Nearbies.FirstAsync(e => e.Id == nearbyId);
        if (partner.Nearbies.Contains(nearby))
        {
            throw new InvalidOperationException($"{nameof(Nearby)} cannot be linked to {nameof(Partner)} because it's already linked.");
        }
        partner.Nearbies.Add(nearby);
        await _db.SaveChangesAsync();
    }

    public async Task UninkAsync(string partnerId, Guid nearbyId)
    {
        Partner partner = await _db.Partners
            .Include(e => e.Nearbies)
            .FirstAsync(e => e.Id == partnerId);
        Nearby nearby = await _db.Nearbies.FirstAsync(e => e.Id == nearbyId);
        if (!partner.Nearbies.Contains(nearby))
        {
            throw new InvalidOperationException($"{nameof(Nearby)} cannot be unlinked from {nameof(Partner)} because it's not linked yet.");
        }
        partner.Nearbies.Remove(nearby);
        await _db.SaveChangesAsync();
    }

    private bool IsSupportedFileType(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return _imageSupportedExtensions.Contains(extension);
    }
}
