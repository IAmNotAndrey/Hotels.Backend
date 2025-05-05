using Hotels.Persistence.Interfaces.Repositories;
using Hotels.Presentation.DtoBs.Images;
using Microsoft.AspNetCore.Http;

namespace Hotels.Persistence.Repositories;

public class ImageRepo : IImageRepo
{
    public IEnumerable<TitledImageDtoB> CreateTitledImageDtos(IEnumerable<IFormFile> images, IEnumerable<bool> areTitle)
    {
        if (images.Count() != areTitle.Count())
        {
            throw new ArgumentException($"The lengths of {nameof(images)} and {nameof(areTitle)} lists must be equal.");
        }

        return images.Zip(areTitle, (file, isTitle) => new TitledImageDtoB { Image = file, IsTitle = isTitle });
    }
}
