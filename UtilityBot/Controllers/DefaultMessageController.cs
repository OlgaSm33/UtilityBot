using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class DefaultMessageController
    {
        private ITelegramBotClient _tgBotClient;
        private IStorage _memoryStorage;

        public DefaultMessageController(ITelegramBotClient tgBotClient, IStorage memoryStorage)
        {
            _tgBotClient = tgBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            string sessionType = _memoryStorage.GetLastSessionType(message.Chat.Id);
            _memoryStorage.Save("Получено не поддерживаемое сообщение", message.Chat.Id, sessionType);
            await _tgBotClient.SendMessage(message.Chat.Id, "Данный тип сообщений не поддерживается. Отправьте текстовое сообщение.", cancellationToken: ct);
        }
    }
}
