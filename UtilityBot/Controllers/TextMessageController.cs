using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;
using UtilityBot.Extensions;


namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private ITelegramBotClient _tgBotClient;
        private IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient tgBotClient, IStorage memoryStorage)
        {
            _tgBotClient = tgBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Текст", $"text"),
                        InlineKeyboardButton.WithCallbackData($"Числа", $"numbers"),
                    });
                    await _tgBotClient.SendMessage(message.Chat.Id, $"<b> Данный бот умеет считать количество символов в текстовых сообщениях и вычислять сумму чисел.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Нажмите кнопку <b>'Текст'</b>, если хотите посчитать символы в сообщениях. {Environment.NewLine} Нажмите кнопку <b>'Числа'</b>, если хотите вычислить сумму чисел", cancellationToken: ct, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    string sessionType = _memoryStorage.GetSession(message.Chat.Id).Type;
                    switch(sessionType)
                    {
                        case "text":
                            await _tgBotClient.SendMessage(message.Chat.Id, "Количество символов в тексте: " + TextHandler.TextLenght(message.Text).ToString(), cancellationToken: ct);
                            break;
                        case "numbers":
                            try
                            {
                                NumbersHandler nh = new NumbersHandler(message.Text.StringToInts());

                                await _tgBotClient.SendMessage(message.Chat.Id, $"Сумма чисел = {nh.SumNumbers()}", cancellationToken: ct);

                            }
                            catch (Exception)
                            {
                                await _tgBotClient.SendMessage(message.Chat.Id, "Вы ввели не последовательность чисел. Попробуйте ещё раз. ", cancellationToken: ct);
                            }
                            break;
                        default:
                            await _tgBotClient.SendMessage(message.Chat.Id, "Отправьте команду '/start' для начала работы с ботом", cancellationToken: ct);
                            break;
                    }
                    break;
            }
        }
    }
}
