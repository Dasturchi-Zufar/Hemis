using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Auth
{
    public class AuthResponse
    {
        public bool success { get; set; }
        public string? error { get; set; }
        public AuthData? data { get; set; }
    }

    public class AuthData
    {
        public string? token { get; set; }=null;
        public string? refresh_token { get; set; }
    }

    public static class AuthService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<AuthResponse> LoginAsync(string login, string password)
        {
           string apiUrl = "https://student.jbnuu.uz/rest/v1/auth/login";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.Add("User-Agent", "CSharpBot/1.0");

    var payload = new { login, password };
    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    var authResponse = new AuthResponse();

    try
    {
        var response = await client.PostAsync(apiUrl, content);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            authResponse.success = false;
            authResponse.error = $"Xato: {response.StatusCode}\n{responseText}";
            return authResponse;
        }

        using var doc = JsonDocument.Parse(responseText);
        var root = doc.RootElement;

        bool success = root.GetProperty("success").GetBoolean();
        authResponse.success = success;

        if (success)
        {
            string token = root.GetProperty("data").GetProperty("token").GetString()!;

              string? refreshToken = null;
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
            refreshToken = cookies.FirstOrDefault(c => c.StartsWith("refresh-token="));
            }

            authResponse.data = new AuthData { 
                token = token 
                , refresh_token = refreshToken
                };

          
        }
        else
        {
            authResponse.error = "Login yoki parol noto‘g‘ri.";
        }

        return authResponse;
    }
    catch (Exception ex)
    {
        authResponse.success = false;
        authResponse.error = $"Xatolik: {ex.Message}";
        return authResponse;
    }
    }
    }
}
