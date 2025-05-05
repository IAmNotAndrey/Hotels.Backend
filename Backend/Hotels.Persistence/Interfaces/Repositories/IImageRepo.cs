using Hotels.Presentation.DtoBs.Images;
using Microsoft.AspNetCore.Http;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IImageRepo
{
    /// <summary>
    /// Создает коллекцию объектов <see cref="TitledImageDtoB"/> на основе переданных файлов изображений <paramref name="images"/> и флагов <paramref name="areTitle"/>.
    /// </summary>
    /// <param name="images">Коллекция файлов изображений.</param>
    /// <param name="areTitle">Коллекция флагов, указывающих, является ли соответствующее изображение заголовком.</param>
    /// <returns>Коллекция объектов <see cref="TitledImageDtoB"/>.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если длины коллекций <paramref name="images"/> и <paramref name="areTitle"/> не совпадают.</exception>
    IEnumerable<TitledImageDtoB> CreateTitledImageDtos(IEnumerable<IFormFile> images, IEnumerable<bool> areTitle);
}
