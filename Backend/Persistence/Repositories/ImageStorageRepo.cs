using Ardalis.GuardClauses;
using AutoMapper;
using Hotels.Application.DtoBs.Images;
using Hotels.Application.Interfaces.Services;
using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hotels.Persistence.Repositories;

public class ImageStorageRepo : IImageStorageRepo
{
    private const string ConfigKeyDirectoryPath = "ImageStorage:DirectoryPath";
    private const string ConfigKeySupportedExtensions = "ImageStorage:SupportedExtensions";

    private readonly HashSet<string> _supportedImageExtensions;

    private readonly ILogger<ImageStorageRepo> _logger;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;
    private readonly IStaticFilesService _staticFilesService;
    private readonly string _galleryDirPath;

    public ImageStorageRepo(ILogger<ImageStorageRepo> logger, IConfiguration configuration, IMapper mapper, IWebHostEnvironment environment, IStaticFilesService staticFilesService)
    {
        _logger = logger;
        _mapper = mapper;
        _environment = environment;
        _staticFilesService = staticFilesService;

        // Проверяем наличие ключа для пути к директории
        var directoryPath = configuration[ConfigKeyDirectoryPath];
        Guard.Against.NullOrWhiteSpace(directoryPath, nameof(directoryPath));
        _galleryDirPath = directoryPath!;

        // Проверяем наличие секции с расширениями и получаем список поддерживаемых расширений
        var supportedExtensionsSection = configuration.GetSection(ConfigKeySupportedExtensions);
        Guard.Against.Null(supportedExtensionsSection, nameof(supportedExtensionsSection));

        _supportedImageExtensions = supportedExtensionsSection.Get<List<string>>()?.ToHashSet()
                                    ?? throw new InvalidOperationException("SupportedExtensions section is missing or invalid.");
    }

    public async Task SaveAndBindImagesAsync<TModel, TImageLink>(
        TModel model,
        IEnumerable<TitledImageDtoB> imageDtos,
        Func<string, TImageLink> imageLinkFactory
    ) where TModel : IHasImageLinks<TImageLink>
      where TImageLink : TitledImageLink
    {
        //string[] savedLinks = await Task.WhenAll(
        //	imageDtos.Select(image => 
        //		SaveFileAsync(image, model.ToString()!)
        //));

        List<TImageLink> imageLinks = [];
        foreach (var dto in imageDtos)
        {
            string path = await SaveFileAsync(dto.Image, model.ToString()!); // Получаем маршрут, по которому сохранилось изображение на Сервере
            TImageLink imageLink = imageLinkFactory(path); // Создаём 'TImageLink' с привязкой к главной модели и 'Uri'
            _mapper.Map(dto, imageLink); // fixme? Устанавливаем оставшиеся свойства по 'Dto': 'IsTitle', ..

            imageLinks.Add(imageLink);
        }

        // Создаём из ссылок модели 'ImageLink'
        // Заменяем изображения на новые
        model.ImageLinks = imageLinks;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string subDirName)
    {
        if (!IsSupportedFileType(file.FileName))
        {
            _logger.LogError("Image type not supported: {FileName}", file.FileName);
            throw new InvalidOperationException($"Image type not supported: {file.FileName}");
        }

        var directoryPath = Path.Combine(_environment.WebRootPath, _galleryDirPath, subDirName);

        return await _staticFilesService.SaveFileAsync(file, directoryPath);
    }

    private bool IsSupportedFileType(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return _supportedImageExtensions.Contains(extension);
    }
}
