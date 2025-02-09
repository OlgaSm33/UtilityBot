using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
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
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7746707429:AAGfOxxEOQmdt90UnQ0ScNF0n62zvfUHTfA"));
            services.AddHostedService<Bot>();

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<TextMessageController>();
        }
    }
}