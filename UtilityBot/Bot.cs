using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Controllers;


namespace UtilityBot
{
    class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        private InlineKeyboardController _inlineKeyoardController;
        private TextMessageController _textMessageController;
        private DefaultMessageController _defaultMessageController;
        public Bot(ITelegramBotClient telegramClient, InlineKeyboardController inlineKeyoardController, TextMessageController textMessageController, DefaultMessageController defaultMessageController)
        {
            _telegramClient = telegramClient;
            _inlineKeyoardController = inlineKeyoardController;
            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } },
                cancellationToken: stoppingToken);
            Console.WriteLine("Бот запущен");
        }
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyoardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }


            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default: 
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }

        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);
            return Task.CompletedTask;
        }
    }
}