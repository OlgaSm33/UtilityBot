using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Controllers
{
    public class DefaultMessageController
    {
        private ITelegramBotClient _tgBotClient;

        public DefaultMessageController(ITelegramBotClient tgBotClient)
        {
            _tgBotClient = tgBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            await _tgBotClient.SendMessage(message.Chat.Id, "Данный тип сообщений не поддерживается. Отправьте текстовое сообщение.", cancellationToken: ct);
        }
    }
}
