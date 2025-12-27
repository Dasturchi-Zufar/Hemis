using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
namespace Handlers;
public static class ImageHandler
{
    public static async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        await bot.SendTextMessageAsync(message.Chat.Id, "Rasm qabul qilindi ✅");
        // keyinchalik rasmni saqlash yoki ishlash mumkin
    }
}
