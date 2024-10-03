using BotApi.Data;
using BotApi.Models;
using BotApi.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi.TgBot.Commands
{
    public class AddNotification(TgBotClient tgBot, ApplicationDbContext context, IUserService userService) : ICommand
    {
        private readonly TelegramBotClient bot = tgBot.Get();
        public string Name { get; set; } = CommandNames.AddNotification;

        private readonly ApplicationDbContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task Execute(Update update)
        {
            var d = update.Message?.Text?[..16]!;
            var dateIsCorrect = DateTime.TryParse(d, out DateTime date);

            if (!dateIsCorrect)
            {
                await bot.SendTextMessageAsync(update.Message?.Chat.Id!, "Неправильная дата");
                return;
            }

            var text = update.Message?.Text?[16..];
            var user = await _userService.GetOrCreate(update);

            await _context.Notification.AddAsync(new Notification
            {
                DateTime = date,
                Text = text!,
                UserId = user.Id
            });

            await _context.SaveChangesAsync();

            var menu = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Список напоминаний", CommandNames.GetNotifications)
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Главное меню", CommandNames.Menu),
                }
            });

            await bot.SendTextMessageAsync(update.Message?.Chat.Id!, "Уведомление добавлено", replyMarkup: menu);
        }
    }
}
