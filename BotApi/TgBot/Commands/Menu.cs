using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi.TgBot.Commands
{
    public class Menu(TgBotClient tgBot) : ICommand
    {
        private readonly TelegramBotClient bot = tgBot.Get();
        public string Name { get; set; } = CommandNames.Menu;

        public async Task Execute(Update update)
        {
            var menu = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Калькулятор", CommandNames.StartCalculator),
                    InlineKeyboardButton.WithCallbackData("Добавить напоминание", CommandNames.StartAddNotification)
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Список предстоящих напоминаний", CommandNames.GetNotifications)
                }
            });

            var text = (update.Type == UpdateType.Message && (bool)(update.Message?.Text?.Contains("/start"))!
                ? "Добро пожаловать! "
                : "") + "Выберите действие:";

            var chatId = update.Type == UpdateType.Message ? update.Message?.Chat.Id! : update.CallbackQuery!.From.Id;

            await bot.SendTextMessageAsync(chatId, text, replyMarkup: menu);
        }
    }
}
