using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi.TgBot
{
    public class TgBotService(IServiceProvider serviceProvider) : ITgBotService
    {
        private readonly List<ICommand> _commands = serviceProvider.GetServices<ICommand>().ToList();
        private ICommand LastCommand { get; set; } = null!;

        public async Task Update(Update update)
        {
            if (update.Type == UpdateType.Message) {
                if ((bool)(update.Message?.Text?.Contains("/start"))!)
                {
                    await ExecuteCommand(CommandNames.Menu, update);
                }
            } else if (update.Type == UpdateType.CallbackQuery)
            {
                switch(update.CallbackQuery?.Data)
                {
                    case CommandNames.Menu:
                        await ExecuteCommand(CommandNames.Menu, update);
                        return;
                    case CommandNames.StartCalculator:
                        await ExecuteCommand(CommandNames.StartCalculator, update);
                        return;
                    case CommandNames.StartAddNotification:
                        await ExecuteCommand(CommandNames.StartAddNotification, update);
                        return;
                    case CommandNames.GetNotifications:
                        await ExecuteCommand(CommandNames.GetNotifications, update);
                        return;
                }
            }

            switch(LastCommand.Name)
            {
                case CommandNames.StartCalculator:
                    await ExecuteCommand(CommandNames.Calculate, update);
                    return;
                case CommandNames.StartAddNotification:
                    await ExecuteCommand(CommandNames.AddNotification, update);
                    return;
            }
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            LastCommand = _commands.FirstOrDefault(c => c.Name == commandName)!;

            await LastCommand.Execute(update);
        }
    }
}
