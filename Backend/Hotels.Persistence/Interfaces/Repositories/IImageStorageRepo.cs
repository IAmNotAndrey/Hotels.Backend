using Hotels.Domain.Common.Interfaces.Images;
using Hotels.Domain.Entities.StaticFiles;
using Hotels.Persistence.Contexts;
using Hotels.Presentation.DtoBs.Images;
using Microsoft.AspNetCore.Http;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IImageStorageRepo
{
    /*
	// removeme	
	/// <summary>
	/// Сохраняет переданные фото для 'saveTo' на сервере
	/// </summary>
	/// <returns>Массив ссылок на фото в сервере</returns>
	Task<string[]> Save(ICollection<IFormFile> files);
	*/

    /// <summary>
    /// Сохраняет изображение в <see cref="ApplicationContext"/> и привязывает к <paramref name="model"/>.
    /// <br/><br/>Если к <paramref name="model"/> уже были привязаны <typeparamref name="TImageLink"/>, то произойдёт конкатенация.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TImageLink"></typeparam>
    /// <param name="imageDtos">'Dto' изображений, которые будут сохранены в <see cref="ApplicationContext"/> и привязаны к <paramref name="model"/>. Все устанавливаемые в 'Dto' свойства будут сохранены.</param>
    /// <param name="imageLinkFactory">Делегат, создающий экземпляр <see cref="TitledImageLink"/>.</param>
    Task SaveAndBindImagesAsync<TModel, TImageLink>(
        TModel model,
        IEnumerable<TitledImageDtoB> imageDtos,
    Func<string, TImageLink> imageLinkFactory
    ) where TModel : IHasImageLinks<TImageLink>
      where TImageLink : TitledImageLink;

    Task<string> SaveFileAsync(IFormFile file, string subDirName);

    /*
    /// <summary>
    /// Удаляет <see cref="IHasImageLinks{T}.ImageLinks"/> из <see cref="ApplicationContext"/>
    /// </summary>
    Task RemoveAsync<TModel, TImageLink>(
        TModel model
    ) where TModel : IHasImageLinks<TImageLink>
      where TImageLink : ImageLink;
    */

    //Task<FileContentResult> GetByUri(string uri);
}
