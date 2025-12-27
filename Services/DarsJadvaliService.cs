using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Storage;
using Models;

namespace Services
{
    public class DarsJadvaliService
    {
        private readonly HttpClient _client = new HttpClient();
       public async Task<List<DarsJadvaliDto>> GetAllAsync(long chatId, int week, int semester)
{
    var auth = TokenStore.Get(chatId);
    if (auth == null || string.IsNullOrEmpty(auth.token))
        return new();

    var url = $"https://student.jbnuu.uz/rest/v1/education/schedule?week={week}&semester={semester}";

    var req = new HttpRequestMessage(HttpMethod.Get, url);
    req.Headers.Add("Accept", "application/json");
    req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.token);

    var res = await _client.SendAsync(req);
    res.EnsureSuccessStatusCode(); // 200 kutilyapti

    var json = await res.Content.ReadAsStringAsync();

    var parsed = JsonSerializer.Deserialize<DarsJadvalResponse>(
        json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    );

    return parsed?.data ?? new();
    }

    }
}
