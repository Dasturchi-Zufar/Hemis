using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using System.Threading.Tasks;
using Services;
using Models;

namespace Handlers
{
    public static class DarsjadHandler
    {
        public static async Task HandleAsync(ITelegramBotClient bot, Message message)
        {
            await bot.SendTextMessageAsync(message.Chat.Id, "⏳ Dars jadvali yuklanmoqda...");
            int sem = UserState.GetSemester(message.Chat.Id);
            var service = new DarsJadvaliService();
            List<DarsJadvaliDto>? darslar = await service.GetAllAsync(message.Chat.Id,0,sem+10);

            if (darslar == null || darslar.Count == 0)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "❌ Darslar topilmadi!");
                return;
            }

            // Hamma darslarni bitta postga chiqaramiz
            var sb = new StringBuilder();
            sb.AppendLine("📚 *Dars Jadvali:* \n");

            var today = DateTime.Today;

// Haftaning boshini topish (Dushanba)
var weekStart = today.AddDays(-(int)today.DayOfWeek + 1);
if (today.DayOfWeek == DayOfWeek.Sunday)
    weekStart = today.AddDays(-6); // Yakshanba bo'lsa, oldingi dushanba

var weekEnd = weekStart.AddDays(6);

// Filtrlash — faqat shu haftaga tegishli darslar
var currentWeekLessons = darslar.Where(d =>
{
    var date = UnixTimeStampToDateTime(d.lesson_date ?? 0).Date;
    return date >= weekStart && date <= weekEnd;
});


        foreach (var d in currentWeekLessons)
         {
         sb.AppendLine($"📘 *Fan:* {d.subject?.name}");
         sb.AppendLine($"👨‍🏫 O‘qituvchi: {d.employee?.name}");
         sb.AppendLine($"🏫 Auditoriya: {d.auditorium?.name}");
         sb.AppendLine($"📅 Sana: {UnixTimeStampToDateTime(d.lesson_date ?? 0):dd.MM.yyyy}");
         sb.AppendLine($"🔢 Juftlik: {d.lessonPair?.name}");
         sb.AppendLine($"— — — — —");
        }

        
        var fullText = sb.ToString();
   await bot.SendTextMessageAsync(
        chatId: message.Chat.Id,
        text: fullText,
        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
    );
        }

        
        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
    // Unix timestamp = seconds since 1970-01-01 UTC
    var dtDateTime = new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc);
    dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
    return dtDateTime;
    }

    }
}
