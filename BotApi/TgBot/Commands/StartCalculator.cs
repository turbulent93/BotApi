using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApi.TgBot.Commands
{
    public class StartCalculator(TgBotClient tgBot) : ICommand
    {
        private readonly TelegramBotClient bot = tgBot.Get();
        public string Name { get; set; } = CommandNames.StartCalculator;

        public async Task Execute(Update update)
        {
            await bot.SendTextMessageAsync(update.CallbackQuery?.From.Id!, "Введите выражение, например: 5 + 5");
        }
    }
}
