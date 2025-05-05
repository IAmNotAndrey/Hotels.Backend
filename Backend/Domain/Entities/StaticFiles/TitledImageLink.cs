namespace Hotels.Domain.Entities.StaticFiles;

public abstract class TitledImageLink : StaticFile
{
    public bool IsTitle { get; set; } = false;
}
