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
            string sessionType = _memoryStorage.GetLastSessionType(message.Chat.Id);
            switch (message.Text)
            {
                case "/start":

                    _memoryStorage.Save("Нажата кнопка '/start'", message.Chat.Id, sessionType);
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

                    _memoryStorage.Save($"Получено текстовое сообщение: {message.Text}", message.Chat.Id, sessionType);
                    switch (sessionType)
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
                                _memoryStorage.Save($"Введены данные, не соответствующие выбранному режиму сессии: {message.Text}", message.Chat.Id, sessionType);
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
