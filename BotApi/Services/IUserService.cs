using BotApi.Models;
using Telegram.Bot.Types;

namespace BotApi.Services
{
    public interface IUserService
    {
        Task<BotApi.Models.User> GetOrCreate(Update update);
    }
}
