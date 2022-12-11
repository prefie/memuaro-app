using Microsoft.AspNetCore.Mvc;
using Rememory.Auth;
using Rememory.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Rememory.WebApi.Controllers;

public class BotController : BaseController
{
    private readonly IBot _bot;
    
    public BotController(
        AuthProvider authProvider,
        IBot bot) : base(authProvider)
    {
        _bot = bot;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            if (update.Message?.Text == "/start")
            {
            }
            await _bot.SendMessage(update.Message!.From!.Id, "answer");
        }
        
        return Ok();
    }
}