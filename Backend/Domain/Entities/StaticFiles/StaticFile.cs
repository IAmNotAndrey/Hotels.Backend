using Hotels.Domain.Common;

namespace Hotels.Domain.Entities.StaticFiles;

public abstract class StaticFile : ApplicationBaseEntity
{
    /// <summary>
    /// Ссылка по которой хранится изображение на сервере
    /// </summary>
    public required string Uri { get; set; }
}
