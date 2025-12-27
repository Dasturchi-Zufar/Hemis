using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;

namespace Handlers;
public static class StartHandler
{
    public static async Task HandleAsync(ITelegramBotClient bot, Message msg)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "🔐 Login"},
            new KeyboardButton[] { "⚙️ Sozlamalar"},
        })
        {
            ResizeKeyboard = true
        };

        await bot.SendTextMessageAsync(
            chatId: msg.Chat.Id,
            text: "Assalomu alaykum!\nTanlang:",
            replyMarkup: keyboard
        );
    }
}
