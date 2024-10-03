using BotApi.Data;
using BotApi.TgBot;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Telegram.Bot;

namespace BotApi.Notifications
{
    public class NotificationSender(ApplicationDbContext context, TgBotClient tgBot) : IJob
    {
        private readonly ApplicationDbContext _context = context;
        private readonly TelegramBotClient bot = tgBot.Get();

        public async Task Execute(IJobExecutionContext context)
        {
            var t = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                0)
                .AddMinutes(10);

            var list = await _context.Notification
                .Where(i => DateTime.Compare(t, i.DateTime) == 0)
                .Include(i => i.User)
                .ToListAsync();

            foreach(var i in list)
            {
                await bot.SendTextMessageAsync(i.User!.ChatId, $"Напоминание: {i.Text} сегодня в {i.DateTime:t}");
            };
        }
    }
}
