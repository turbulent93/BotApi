
using Telegram.Bot.Types;

namespace BotApi.TgBot
{
    public interface ITgBotService
    {
        Task Update(Update update);
    }
}
