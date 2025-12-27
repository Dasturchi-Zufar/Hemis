using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks; 
using Services;
using Models;
using Services.Auth;
using Telegram.Bot.Types.ReplyMarkups;
using Storage;
namespace Handlers
{
    public static class LoginHandler
    {
        
        public static async Task HandleAsync(ITelegramBotClient bot, Message msg)
        {
            long chatId = msg.Chat.Id;
            string text = msg.Text!.Trim();

            // Boshlanish: /login komanda kelganda
            if (text == "🔐 Login")
            {
                UserStateService.States[chatId] = LoginState.WaitingLogin;
                UserStateService.TempLogin.TryRemove(chatId, out _);

                await bot.SendTextMessageAsync(chatId, "🔑 Login kiriting:");
                return;
            }

            // Hozirgi state
            UserStateService.States.TryGetValue(chatId, out var state);

            switch (state)
            {
                case LoginState.WaitingLogin:
                    await HandleLogin(bot, msg);
                    break;

                case LoginState.WaitingPassword:
                    await HandlePassword(bot, msg);
                    break;

                default:
                    await bot.SendTextMessageAsync(chatId, "❗ Avval /login buyrug‘ini bering.");
                    break;
            }
        }
         private static async Task HandleLogin(ITelegramBotClient bot, Message msg)
        {
            long chatId = msg.Chat.Id;
            string login = msg.Text!.Trim();

            if (string.IsNullOrWhiteSpace(login))
            {
                await bot.SendTextMessageAsync(chatId, "❗ Login bo‘sh bo‘lmasin. Qayta kiriting:");
                return;
            }

            UserStateService.TempLogin[chatId] = login;
            UserStateService.States[chatId] = LoginState.WaitingPassword;

            await bot.SendTextMessageAsync(chatId, "🔐 Parol kiriting:");
        }
        private static async Task HandlePassword(ITelegramBotClient bot, Message msg)
        {
            long chatId = msg.Chat.Id;
            string password = msg.Text!.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                await bot.SendTextMessageAsync(chatId, "❗ Parol bo‘sh bo‘lmasin. Qayta kiriting:");
                return;
            }

            // Saqlangan login
            UserStateService.TempLogin.TryGetValue(chatId, out string? login);
            
            var result=await AuthService.LoginAsync(login!, password);
           if (result == null || result.success == false)
           {
                await bot.SendTextMessageAsync(chatId, "❌ Login yoki parol xato!");
                return;
           }

            var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Dars jadvali", "Davomat" },
            new KeyboardButton[] { "Menu","Mening ma'lumotlarim" },
        })
        {
            ResizeKeyboard = true
        };
        
                TokenStore.Set(chatId, result.data!.token, result.data.refresh_token);
                
                await bot.SendTextMessageAsync(chatId, 
                text:
                $"✅ Muvaffaqiyatli tizimga kirdingiz!{result.data!.token}{result.data.refresh_token}",
                replyMarkup:keyboard);
              var service=new InfoService();
var info=await service.GetInfoAsync(msg.Chat.Id);
           var root = info.Value;
string semester = root.GetProperty("semester").GetProperty("name").GetString()!;
UserState.SetSemester(chatId, semester);
            // tozalash
            UserStateService.States[chatId] = LoginState.None;
            UserStateService.TempLogin.TryRemove(chatId, out _);
        }
            
    }
}