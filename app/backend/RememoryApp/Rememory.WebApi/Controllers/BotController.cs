using Microsoft.AspNetCore.Mvc;
using Rememory.Auth;
using Rememory.Bot;
using Rememory.Persistance.Repositories.NotificationSettingsRepository;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Rememory.WebApi.Controllers;

public class BotController : BaseController
{
    private readonly IBot _bot;
    private readonly INotificationSettingsRepository _notificationSettingsRepository;
    
    public BotController(
        AuthProvider authProvider,
        IBot bot,
        INotificationSettingsRepository notificationSettingsRepository) : base(authProvider)
    {
        _bot = bot;
        _notificationSettingsRepository = notificationSettingsRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            if (update.Message?.Text == "/start")
            {
                var settings = await _notificationSettingsRepository.GetByTelegramName(update.Message!.From!.Username);
                if (settings == null)
                {
                    await _bot.SendMessage(update.Message!.From!.Id, "Настрой уведомления через Rememory и снова введи /start");
                }
                else
                {
                    settings.TelegramId = update.Message!.From!.Id.ToString();
                    await _notificationSettingsRepository.UpdateAsync(settings.Id, settings);
                    await _bot.SendMessage(update.Message!.From!.Id, "Теперь ты будешь получать уведомления от Rememory в этот чат");
                }
            }
            else
            {
                await _bot.SendMessage(update.Message!.From!.Id, "Я умею только присылать уведомления. Чтобы отказаться от них, поменяй настройки в Rememory");
            }
        }
        
        return Ok();
    }
}