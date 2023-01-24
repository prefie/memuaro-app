using Rememory.Email.Settings;

namespace Rememory.Email;

public interface IEmailClient
{
    public EmailSettings EmailSettings { get; }
    public Task SendMessage(string email, string message);
    public Task SendMessageWithAttachments(string email, string message, Stream attachments, string fileName);
}