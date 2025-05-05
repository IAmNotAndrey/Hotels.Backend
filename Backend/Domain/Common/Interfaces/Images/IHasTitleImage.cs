using Hotels.Domain.Entities.StaticFiles;

namespace Hotels.Domain.Common.Interfaces.Images;

public interface IHasTitleImage<T> : IHasImageLinks<T> where T : TitledImageLink
{
    public T? TitleImageLink { get; }
}
