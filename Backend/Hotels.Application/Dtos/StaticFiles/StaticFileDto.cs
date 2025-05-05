using Hotels.Application.Dtos.Common;

namespace Hotels.Application.Dtos.StaticFiles;

public abstract class StaticFileDto : ApplicationBaseEntityDto
{
    public string Uri { get; set; } = null!;
}
