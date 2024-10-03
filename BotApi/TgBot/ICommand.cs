using Telegram.Bot.Types;

namespace BotApi.TgBot
{
    public interface ICommand
    {
        string Name { get; set; }
        Task Execute(Update update);
    }
}
