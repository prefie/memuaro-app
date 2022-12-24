using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Rememory.Email.Settings;

namespace Rememory.Email;

public class EmailClient : IEmailClient
{
    private readonly EmailSettings _emailSettings;

    public EmailClient(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendMessage(string email, string message)
    {
        if (_emailSettings.Email == null || _emailSettings.Password == null) throw new ArgumentException("Bad email settings");

        var mimeMessage = new MimeMessage();
        var from = new MailboxAddress("Rememory.Notifications", "rememory.notifications@yandex.ru");
        mimeMessage.From.Add(from);
        var to = new MailboxAddress(email, email);
        mimeMessage.To.Add(to);
        mimeMessage.Subject = "Новое уведомление из Rememory";
        
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.yandex.ru", 465, true);
        await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}