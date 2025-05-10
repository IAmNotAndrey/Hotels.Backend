using Ardalis.GuardClauses;
using AutoMapper;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Entities;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hotels.Infrastructure.Services;

public class CafeService : ICafeService
{
    private const string ConfigKeyStaticFilesDirPath = "StaticFiles:DirectoryPath";
    private const string ConfigKeyMenuDirPath = "StaticFiles:CafeMenu:DirectoryPath";
    private const string ConfigKeyMenuSupportedExtensions = "StaticFiles:CafeMenu:SupportedExtensions";

    private readonly ApplicationContext _db;
    private readonly IMapper _mapper;
    private readonly IStaticFilesService _staticFilesService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;
    private readonly string _staticFilesDirPath;
    private readonly string _menuDirPath;
    private readonly HashSet<string> _menuSupportedExtensions;

    public CafeService(ApplicationContext db, IMapper mapper, IStaticFilesService staticFilesService, IWebHostEnvironment environment, IConfiguration configuration, ILogger<CafeService> logger)
    {
        _db = db;
        _mapper = mapper;
        _staticFilesService = staticFilesService;
        _environment = environment;
        _logger = logger;

        // Get data from config
        var staticFilesDirPath = configuration[ConfigKeyStaticFilesDirPath];
        Guard.Against.NullOrWhiteSpace(staticFilesDirPath, message: $"The '{nameof(staticFilesDirPath)}' wasn't found by the path '{ConfigKeyStaticFilesDirPath}'.");
        _staticFilesDirPath = staticFilesDirPath;

        var menuDirPath = configuration[ConfigKeyMenuDirPath];
        Guard.Against.NullOrWhiteSpace(menuDirPath, message: $"The '{nameof(menuDirPath)}' wasn't found by the path '{ConfigKeyMenuDirPath}'.");
        _menuDirPath = menuDirPath;
        // Get extensions from config.
        var supportedMenuFileExtensions = configuration.GetSection(ConfigKeyMenuSupportedExtensions);
        Guard.Against.Null(supportedMenuFileExtensions, nameof(supportedMenuFileExtensions), message: $"The '{supportedMenuFileExtensions}' wasn't found by the path {ConfigKeyMenuSupportedExtensions}.");
        _menuSupportedExtensions = supportedMenuFileExtensions.Get<List<string>>()?.ToHashSet()
            ?? throw new InvalidOperationException("SupportedExtensions section is missing or invalid.");
    }

    public async Task SaveMenuFileAsync(Guid id, IFormFile menuFile)
    {
        var cafe = await _db.Cafes
            .Include(e => e.Menu)
            .FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new ArgumentException($"{nameof(Cafe)} wasn't found by id '{id}'", nameof(id));

        // Check whether `menuFile` has suitable extension.
        if (!IsSupportedFileType(menuFile.FileName))
        {
            _logger.LogError("Menu file extension not supported: {FileName}", menuFile.FileName);
            throw new InvalidOperationException($"Menu file extension not supported: {menuFile.FileName}");
        }
        // Create full directory path to save menu file.
        string dirPath = Path.Combine(_environment.WebRootPath, _staticFilesDirPath, _menuDirPath, cafe.ToString());
        // Get full path of the saved file.
        var fullPath = await _staticFilesService.SaveFileAsync(menuFile, dirPath);

        // Set new menu link deleting existing.
        CafeMenuFileLink menuLink = new() { CafeId = cafe.Id, Uri = fullPath };
        _db.CafeMenuFileLinks.Add(menuLink);

        await _db.SaveChangesAsync();
    }

    private bool IsSupportedFileType(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return _menuSupportedExtensions.Contains(extension);
    }
}
