using Microsoft.Extensions.Options;
using Rememory.Bot.Settings;
using Telegram.Bot;

namespace Rememory.Bot;

public class TelegramBot : IBot
{
    private readonly TelegramBotClient _client;

    public TelegramBot(IOptions<BotSettings> settings)
    {
        _client = new TelegramBotClient(settings.Value.Token ?? string.Empty);
    }

    public async Task SendMessage(long id, string text)
    {
        var message = await _client.SendTextMessageAsync(id, text);
    }

    public async Task SendMessage(string name, string text)
    {
        var message = await _client.SendTextMessageAsync(name, text);
    }
}