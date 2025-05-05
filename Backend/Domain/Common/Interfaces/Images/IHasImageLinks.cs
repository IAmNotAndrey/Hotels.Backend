using Hotels.Domain.Entities.StaticFiles;

namespace Hotels.Domain.Common.Interfaces.Images;

public interface IHasImageLinks<T> where T : TitledImageLink
{
    ICollection<T> ImageLinks { get; set; }
}
