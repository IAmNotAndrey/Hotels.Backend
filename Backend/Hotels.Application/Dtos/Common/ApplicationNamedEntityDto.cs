namespace Hotels.Application.Dtos.Common;

public abstract class ApplicationNamedEntityDto : ApplicationBaseEntityDto
{
    public string Name { get; set; } = null!;
}
