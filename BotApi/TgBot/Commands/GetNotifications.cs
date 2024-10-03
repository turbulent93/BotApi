using BotApi.Data;
using BotApi.Models;
using BotApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi.TgBot.Commands
{
    public class GetNotifications(TgBotClient tgBot, ApplicationDbContext context, IUserService userService) : ICommand
    {
        private readonly TelegramBotClient bot = tgBot.Get();
        public string Name { get; set; } = CommandNames.GetNotifications;

        private readonly ApplicationDbContext _context = context;
        private readonly IUserService _userService = userService;

        public async Task Execute(Update update)
        {
            var user = await _userService.GetOrCreate(update);

            var list = await _context.Notification
                .Where(i => i.UserId == user.Id && DateTime.Compare(i.DateTime, DateTime.Now) > 0)
                .ToListAsync();
            
            var menu = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Главное меню", CommandNames.Menu),
                }
            });

            await bot.SendTextMessageAsync(
                update.CallbackQuery?.From.Id!,
                $"Предстоящие напоминания:\n{string.Join('\n', list.Select(i => $"{i.DateTime:g}\t{i.Text}"))}",
                replyMarkup: menu);
        }
    }
}
