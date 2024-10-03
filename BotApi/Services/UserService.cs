using BotApi.Data;
using BotApi.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace BotApi.Services
{
    public class UserService(ApplicationDbContext context) : IUserService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<BotApi.Models.User> GetOrCreate(Update update)
        {
            var ci = update.Message?.Chat.Id != null
                ? update.Message.Chat.Id.ToString()
                : update.CallbackQuery?.From.Id.ToString();

            var user = await _context.User.FirstOrDefaultAsync(i => i.ChatId == ci);

            if(user == null)
            {
                user = new Models.User
                {
                    ChatId = update.Message!.Chat.Id.ToString()
                };

                await _context.User.AddAsync(user);

                await _context.SaveChangesAsync();
            }

            return user;
        }
    }
}
