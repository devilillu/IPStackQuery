using Helper;
using IPQuery;
using Microsoft.AspNetCore.Http;

namespace IPStack;

public static class CacheQueryCom
{
    public static string SetPortPattern => "/api/set/port/{" + Port + "}";

    static string Port => "port";

    public static void SetPortHandler(HttpContext context) => PortNo = context.RetrieveFromUri(Port);

    public static string PortNo { get; private set; } = "5001";

    public static async Task<string> IsAvailble(IPQuerySeed queryInfo) =>
         await HttpClientHelper.GetAsync(BuildCheckCacheUri(queryInfo.Ip));

    public static async void UpdateCache(IPQueryResult result) =>
        await HttpClientHelper.PostJsonAsync(BuildUpdateCacheUri(result.Info.Ip), result.Result.ToJsonString());

    static string BuildCheckCacheUri(string ip) => $"http://localhost:{PortNo}/api/check/{ip}";

    static string BuildUpdateCacheUri(string ip) => $"http://localhost:{PortNo}/api/cache/{ip}";

}

