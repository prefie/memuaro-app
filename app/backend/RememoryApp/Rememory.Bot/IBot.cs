namespace Rememory.Bot;

public interface IBot
{
    public Task SendMessage(long id, string message);
    public Task SendMessage(string? name, string message);
}