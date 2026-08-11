using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Identity;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GymMangamentSystem.Reposatory.Services.Auth;

public sealed class SmtpEmailService : IEmailService
{
    private readonly MailSettings _mailSettings;

    public SmtpEmailService(IOptionsMonitor<MailSettings> options)
    {
        _mailSettings = options.CurrentValue;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _mailSettings.DisplayedName,
            _mailSettings.Email));
        message.To.Add(new MailboxAddress(string.Empty, to));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = body
        };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(
            _mailSettings.SmtpServer,
            _mailSettings.Port,
            SecureSocketOptions.StartTls,
            cancellationToken);
        await client.AuthenticateAsync(
            _mailSettings.Email,
            _mailSettings.Password,
            cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
