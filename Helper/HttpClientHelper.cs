using System.Text;

namespace Helper;

public static class HttpClientHelper
{
    public static async Task<string> GetAsync(string uri)
    {
        using var response = await new HttpClient().GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<HttpResponseMessage> PostJsonAsync(string uri, string jsonObject)
    {
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        return await new HttpClient().PostAsync(uri, content);
    }
}
