using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using Services;
using Models;
namespace Handlers;
public static class TextHandler
{
    public static async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        long chatId = message.Chat.Id;
        string text = message.Text ?? "";

        // 1️⃣ Login jarayoni davom etayotgan bo‘lsa → switch emas!
        UserStateService.States.TryGetValue(chatId, out var state);

        if (state == LoginState.WaitingLogin || state == LoginState.WaitingPassword)
        {
            await LoginHandler.HandleAsync(bot, message);
            return;
        }

       switch (text)
        {
            case "/start":
                await StartHandler.HandleAsync(bot, message);
                break;
            case "/login":
            case "🔐 Login":
                await LoginHandler.HandleAsync(bot, message);
                break;
            case "Dars jadvali":
               await DarsjadHandler.HandleAsync(bot, message);
                break;
            case "Mening ma'lumotlarim":
                await InfoHandler.HandleAsync(bot, message);
                break;
            case "Davomat":
                await DavomatHandler.HandleAsync(bot, message);
                break;
            case "Menu":
             UserStateService.States[chatId] = LoginState.None;
            UserStateService.TempLogin.TryRemove(chatId, out _);
                await StartHandler.HandleAsync(bot, message);
                break;
            default:
                await bot.SendTextMessageAsync(chatId, "❗ Buyruq topilmadi!");
                break;
        }
    }
}
