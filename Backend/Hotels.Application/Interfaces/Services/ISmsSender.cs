namespace Hotels.Application.Interfaces.Services;

public interface ISmsSender
{
    Task SendSmsAsync(string number, string text);
}
