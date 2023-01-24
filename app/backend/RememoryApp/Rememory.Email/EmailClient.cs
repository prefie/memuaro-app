using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Rememory.Email.Settings;

namespace Rememory.Email;

public class EmailClient : IEmailClient
{
    public EmailSettings EmailSettings { get; }

    public EmailClient(IOptions<EmailSettings> emailSettings)
    {
        EmailSettings = emailSettings.Value;
    }

    public async Task SendMessage(string email, string message)
    {
        if (EmailSettings.Email == null || EmailSettings.Password == null)
            throw new ArgumentException("Bad email settings");

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
        await client.AuthenticateAsync(EmailSettings.Email, EmailSettings.Password);
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }

    public async Task SendMessageWithAttachments(string email, string message, Stream attachments, string fileName)
    {
        if (EmailSettings.Email == null || EmailSettings.Password == null)
            throw new ArgumentException("Bad email settings");

        var mimeMessage = new MimeMessage();
        var from = new MailboxAddress("Rememory.Notifications", "rememory.notifications@yandex.ru");
        mimeMessage.From.Add(from);
        var to = new MailboxAddress(email, email);
        mimeMessage.To.Add(to);
        mimeMessage.Subject = "Новый запрос на книгу из Rememory";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message
        };

        var attachment = new MimePart("answers", "pdf")
        {
            Content = new MimeContent(attachments),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = fileName
        };

        var multipart = new Multipart("mixed");
        multipart.Add(bodyBuilder.ToMessageBody());
        multipart.Add(attachment);

        mimeMessage.Body = multipart;

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.yandex.ru", 465, true);
        await client.AuthenticateAsync(EmailSettings.Email, EmailSettings.Password);
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}