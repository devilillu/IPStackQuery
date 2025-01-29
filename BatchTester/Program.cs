using Helper;
using System.Text.Json;

namespace BatchTester;

internal class Program
{
    static async Task Main(string[] args)
    {
        Random seed = new();

        int noOfIPs = Convert.ToInt32(args[0]);
        int port = Convert.ToInt32(args[1]);

        string uri = $"http://localhost:{port}/api/batch";

        var ips = Enumerable.Range(0, noOfIPs).Select(x => $"{x}.{seed.Next(256)}.{seed.Next(256)}.{seed.Next(256)}");

        var response = await HttpClientHelper.PostJsonAsync(uri, JsonSerializer.Serialize(ips.ToList()));
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}
