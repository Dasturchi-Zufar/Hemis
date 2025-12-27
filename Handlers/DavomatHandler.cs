using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using Services;
using System.Text.Json;
using System.Text;
using Storage;
using Models;
namespace Handlers{
public static class DavomatHandler
{
    public static async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
      
       long chatId = message.Chat.Id;

            int sem = UserState.GetSemester(chatId);
            var dataArray = await DavomatService.GetAttendanceAsync(chatId, sem+10);

            if (dataArray == null)
            {
                await bot.SendTextMessageAsync(chatId, "Davomat topilmadi ❌");
                return;
            }

            if (dataArray.Value.GetArrayLength() == 0)
            {
                await bot.SendTextMessageAsync(chatId, "Davomat maʼlumotlari mavjud emas.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("📊 *Davomat maʼlumotlari*");
            sb.AppendLine("──────────────────────");

            foreach (var item in dataArray.Value.EnumerateArray())
            {
                 string fan = item.GetProperty("subject").GetProperty("name").GetString() ?? "Nomaʼlum fan";
    string code = item.GetProperty("subject").GetProperty("code").GetString() ?? "";
    string employee = item.GetProperty("employee").GetProperty("name").GetString() ?? "Nomaʼlum o‘qituvchi";
    int absentOff = item.GetProperty("absent_off").GetInt32();

    // Sana: Unix timestamp -> DateTime
    long lessonDateUnix = item.GetProperty("lesson_date").GetInt64();
    DateTime lessonDate = DateTimeOffset.FromUnixTimeSeconds(lessonDateUnix).DateTime;
    string formattedDate = lessonDate.ToString("dd.MM.yyyy"); // 18.11.2025

    sb.AppendLine($"📘 *Fan:* {fan} ({code})");
    sb.AppendLine($"👨‍🏫 *O‘qituvchi:* {employee}");
    sb.AppendLine($"📅 *Sana:* {formattedDate}");
    sb.AppendLine($"❌ *Yo‘qlik:* {absentOff}");
    sb.AppendLine("──────────────────────");
            }

            await bot.SendTextMessageAsync(
                chatId,
                sb.ToString(),
                Telegram.Bot.Types.Enums.ParseMode.Markdown
            );


    }
}
}