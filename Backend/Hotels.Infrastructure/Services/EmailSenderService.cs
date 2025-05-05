using Ardalis.GuardClauses;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Hotels.Infrastructure.Services;

public class EmailSenderService : IEmailSender
{
    private const string ConfigKeyFromEmail = "EmailSender:FromEmail";
    private const string ConfigKeyFromPassword = "EmailSender:FromPassword";
    private const string ConfigKeyFromName = "EmailSender:FromName";
    private const string ConfigKeyHost = "EmailSender:Host";
    private const string ConfigKeyPort = "EmailSender:Port";
    private const string ConfigKeyUseSsl = "EmailSender:UseSsl";

    private readonly ILogger<EmailSenderService> _logger;
    private readonly string _fromEmail;
    private readonly string _fromPassword;
    private readonly string _fromName;
    private readonly string _host;
    private readonly int _port;
    private readonly bool _useSsl;

    public EmailSenderService(ILogger<EmailSenderService> logger, IConfiguration configuration)
    {
        _logger = logger;

        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyFromEmail]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyFromPassword]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyFromName]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyHost]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyPort]);
        Guard.Against.NullOrWhiteSpace(configuration[ConfigKeyUseSsl]);

        _fromEmail = configuration[ConfigKeyFromEmail]!;
        _fromPassword = configuration[ConfigKeyFromPassword]!;
        _fromName = configuration[ConfigKeyFromName]!;
        _host = configuration[ConfigKeyHost]!;
        _port = int.Parse(configuration[ConfigKeyPort]!);
        _useSsl = bool.Parse(configuration[ConfigKeyUseSsl]!);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_fromName, _fromEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlMessage
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_host, _port, _useSsl);
            await client.AuthenticateAsync(_fromEmail, _fromPassword);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        };

        _logger.LogInformation("The message was sent to the '{Email}'", email);
    }
}
