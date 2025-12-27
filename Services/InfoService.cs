using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Storage;
namespace Services
{
    public class InfoService
    {
        private readonly HttpClient _client = new HttpClient();
        public  async Task<JsonElement?> GetInfoAsync(long chatId)
        {
            var auth = TokenStore.Get(chatId);
            if (auth == null || string.IsNullOrEmpty(auth.token))
                return null;

            var url = "https://student.jbnuu.uz/rest/v1/account/me";

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Add("Accept", "application/json");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.token);

            var res = await _client.SendAsync(req);
            res.EnsureSuccessStatusCode(); // 200 kutilyapti

            var json = await res.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);

            return doc.RootElement.GetProperty("data");
        }
    }
}
