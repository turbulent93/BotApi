using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApi.TgBot.Commands
{
    public class StartAddNotification(TgBotClient tgBot) : ICommand
    {
        private readonly TelegramBotClient bot = tgBot.Get();
        public string Name { get; set; } = CommandNames.StartAddNotification;

        public async Task Execute(Update update)
        {
            await bot.SendTextMessageAsync(
                update.CallbackQuery?.From.Id!,
                "Введите уведомление в виде - дд.мм.гггг чч:мм текст уведомления");
        }
    }
}
