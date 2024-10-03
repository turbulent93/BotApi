using BotApi.TgBot;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace BotApi.Controllers
{
    [ApiController]
    [Route("bot")]
    public class TgBotController(ITgBotService tgBotService) : ControllerBase
    {
        private readonly ITgBotService _tgBotService = tgBotService;
        
        [HttpPost("update")]
        public async Task Update(Update update)
        {
            await _tgBotService.Update(update);
        }

        [HttpGet]
        public string Get()
        {
            return "Welcome!";
        }
    }
}
