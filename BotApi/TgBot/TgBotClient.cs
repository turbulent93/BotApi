using System.Configuration;
using Telegram.Bot;

namespace BotApi.TgBot
{
    public class TgBotClient(IConfiguration configuration)
    {
        private readonly TgBotSettings _settings = configuration.GetSection(nameof(TgBotSettings)).Get<TgBotSettings>()!;
        private TelegramBotClient _bot = null!;

        public TelegramBotClient Get()
        {
            _bot = new TelegramBotClient(_settings.Token);

            return _bot;
        }

        public async Task SetWebhook()
        {
            if (_bot == null)
                Get();

            var info = await _bot!.GetWebhookInfoAsync();

            if (info!.Url != _settings.UpdateUrl)
                await _bot!.SetWebhookAsync(_settings.UpdateUrl);
        }
    }
}
