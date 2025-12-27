using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Storage;

namespace Services
{
    public  class DavomatService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // Global token
       

        public static async Task<JsonElement?> GetAttendanceAsync(long chatid,int semester )
        {
            
            try
            {
                 var Token = TokenStore.Get(chatid);
              if (Token == null || string.IsNullOrEmpty(Token.token))
        {
            Console.WriteLine("Token topilmadi yoki bo'sh!");
            return null;
        }

                string url = $"https://student.jbnuu.uz/rest/v1/education/attendance?subject={0}&semester={semester}";

                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.token);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);
                var dataArray = doc.RootElement.GetProperty("data");
                
                return dataArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attendance: {ex.Message}");
                return null;
            }
        }
    }
}
