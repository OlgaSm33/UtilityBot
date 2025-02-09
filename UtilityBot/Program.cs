using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using UtilityBot.Configuration;
using UtilityBot.Controllers;
using UtilityBot.Services;


namespace UtilityBot
{
    static class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();
            Console.WriteLine("Сервис запущен");
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");

        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAddSettings();
            services.AddSingleton(appSettings);

            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.botToken));
            services.AddHostedService<Bot>();

            services.AddSingleton<IStorage, FileMemoryStorage>();

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<TextMessageController>();
        }

        static AppSettings BuildAddSettings()
        {
            return new AppSettings()
            {
                botToken = "7746707429:AAGfOxxEOQmdt90UnQ0ScNF0n62zvfUHTfA",
                logFileName = "logs",
                logFileFormat = "txt",
                logFilePath = "C:\\VS\\UtilityBot"

            };
        }
    }
}