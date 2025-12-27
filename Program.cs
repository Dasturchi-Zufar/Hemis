using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Configuration;
using Handlers;
namespace Bots
{
    class Program
    {
        static TelegramBotClient bot = null!;
        static void Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            bot = new TelegramBotClient(config["8550496090:AAF4cyinFFafKmABoGXPk9o2Y5PiElSXelo"]!);
            using var cts = new CancellationTokenSource();
            bot.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,

                new Telegram.Bot.Polling.ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                },
                cancellationToken: cts.Token
            );

            Console.WriteLine("Bot is running. Press any key to exit");
            Console.ReadKey();

            cts.Cancel();

        }
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (bot == null) { Console.WriteLine("bot null!"); return; }
            if (update == null) { Console.WriteLine("update null!"); return; }
            try
            {
                if (update.Type == UpdateType.Message && update.Message != null)
                {
                    var message = update.Message;

                    if (message.Type == MessageType.Text)
                    {
                        await Handlers.TextHandler.HandleAsync(botClient, message);
                    }
                    else if (message.Type == MessageType.Photo)
                    {
                        await Handlers.ImageHandler.HandleAsync(botClient, message);
                    }
                    // boshqa xabar turlari uchun holatlar qo'shish mumkin
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xato: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
        static async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error occurred: {exception.Message}");
            await Task.CompletedTask;
        }


    }
}