using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;


namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private ITelegramBotClient _tgBotClient;
        private IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient tgBotClient, IStorage memoryStorage)
        {
            _tgBotClient = tgBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;
            _memoryStorage.GetSession(callbackQuery.From.Id).Type = callbackQuery.Data;

            string sessionType = callbackQuery.Data switch
            {
                "text" => "Текст",
                "numbers" => "Числа",
                _ => String.Empty
            };
            await _tgBotClient.SendMessage(callbackQuery.From.Id, $"<b>Вы выбрали режим - {sessionType}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
