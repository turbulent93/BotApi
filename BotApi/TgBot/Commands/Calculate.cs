using System.Buffers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi.TgBot.Commands
{
    public class Calculate(TgBotClient tgBot) : ICommand
    {
        private readonly TelegramBotClient bot = tgBot.Get();
        public string Name { get; set; } = CommandNames.Calculate;

        private List<string> Operators { get; set; } = ["+", "-", "*", "/"];

        public async Task Execute(Update update)
        {
            string[] exp = update.Message?.Text!.Split(' ')!;

            if (!int.TryParse(exp[0], out int fo) || !Operators.Contains(exp[1]) || !int.TryParse(exp[2], out int lo))
                await bot.SendTextMessageAsync(update.Message?.Chat.Id!, $"Неправильное выражение");

            var firstOp = int.Parse(exp[0]);
            var lastOp = int.Parse(exp[2]);
            int? result = null;

            switch(exp[1])
            {
                case "+":
                    result = firstOp + lastOp;
                    break;
                case "-":
                    result = firstOp - lastOp;
                    break;
                case "*":
                    result = firstOp * lastOp;
                    break;
                case "/":
                    result = firstOp / lastOp;
                    break;
            }

            var menu = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Главное меню", CommandNames.Menu),
                }
            });

            await bot.SendTextMessageAsync(update.Message?.Chat.Id!, $"Результат - {result}", replyMarkup: menu);
        }
    }
}
