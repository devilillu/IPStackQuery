using IPQuery;
using Helper;

namespace IPStack;

public static class QueryCom
{
    public static async Task<IPQueryResult> QueryIPStack(IPQuerySeed query, string token)
    {
        try
        {
            return new(query, new(DateTime.Now, await HttpClientHelper.GetAsync(BuildDefault(query.Ip, token))));
        }
        catch (Exception ex)
        {
            throw new IPServiceNotAvailableException(ex.Message);
        }
    }

    public static async Task<IPQueryResult> QueryDev(IPQuerySeed query)
    {
        await Task.Delay(100);
        return new(query, new(DateTime.Now, "Dev details"));
    }

    internal static string BuildDefaultEmtpy(string ip, string accessKey) =>
        $"http://api.ipstack.com/{ip}";

    internal static string BuildDefault(string ip, string accessKey) =>
        $"http://api.ipstack.com/{ip}?access_key={accessKey}&output=json";
}

public class IPServiceNotAvailableException(string message) : Exception(message) { }