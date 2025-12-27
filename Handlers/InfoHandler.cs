using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks; 
using System.Text.Json;
using Services;
namespace Handlers
{
    public static class InfoHandler
    {
        public static async Task HandleAsync(ITelegramBotClient bot, Message msg)
        {
            var service=new InfoService();
            var info=await service.GetInfoAsync(msg.Chat.Id);
           var root = info.Value;
           long chatId = msg.Chat.Id;
string semester = root.GetProperty("semester").GetProperty("name").GetString()!;
UserState.SetSemester(chatId, semester);

string imageUrl = root.GetProperty("image").GetString()!;
string text = 
$@"🎓 *Talaba ma'lumotlari*

👤 *F.I.Sh:* {root.GetProperty("full_name").GetString()}
🆔 *ID raqami:* {root.GetProperty("student_id_number").GetString()}
📞 *Telefon:* {root.GetProperty("phone").GetString()}
📧 *Email:* {root.GetProperty("email").GetString()}
🧬 *Jinsi:* {root.GetProperty("gender").GetProperty("name").GetString()}

🏫 *O‘quv yurt:* {root.GetProperty("university").GetString()}
📚 *Mutaxassislik:* {root.GetProperty("specialty").GetProperty("name").GetString()}
🔢 *Kode:* {root.GetProperty("specialty").GetProperty("code").GetString()}
👥 *Guruh:* {root.GetProperty("group").GetProperty("name").GetString()}
🎓 *Kurs:* {root.GetProperty("level").GetProperty("name").GetString()}
📅 *Semestr:* {root.GetProperty("semester").GetProperty("name").GetString()}
📘 *O‘quv yili:* {root.GetProperty("semester").GetProperty("education_year").GetProperty("name").GetString()}

📊 *O‘rtacha GPA:* {root.GetProperty("avg_gpa").GetString()}
📄 *Status:* {root.GetProperty("studentStatus").GetProperty("name").GetString()}
🏷 *Ta’lim shakli:* {root.GetProperty("educationForm").GetProperty("name").GetString()}
🎓 *Ta’lim turi:* {root.GetProperty("educationType").GetProperty("name").GetString()}
💳 *To‘lov turi:* {root.GetProperty("paymentForm").GetProperty("name").GetString()}

🌍 *Manzil:*  
{root.GetProperty("country").GetProperty("name").GetString()},  
{root.GetProperty("province").GetProperty("name").GetString()},  
{root.GetProperty("district").GetProperty("name").GetString()},  
{root.GetProperty("address").GetString()}

🔗 *Tasdiqlash havolasi:*  
{root.GetProperty("validateUrl").GetString()}
";

            await bot.SendPhotoAsync(
    chatId: msg.Chat.Id,
    photo: imageUrl,
    caption: text,
    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
);
        }
    }
}