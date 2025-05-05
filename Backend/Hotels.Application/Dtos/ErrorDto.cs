using System.Text.Json;

namespace Hotels.Application.Dtos;

public class ErrorDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
