namespace Rememory.Email;

public interface IEmailClient
{
    public Task SendMessage(string email, string message);
}