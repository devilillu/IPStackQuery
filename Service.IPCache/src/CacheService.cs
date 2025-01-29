using Helper;
using IPQuery;

namespace Service.IPCache;

internal static class CacheService
{
    public static IPMemoryCache IPMemory = new();

    public static string IpCachePattern => "/api/cache/{" + IpId + "}";

    public static string IpCheckPattern => "/api/check/{" + IpId + "}";

    static string IpId => "ip";

    public static async Task CheckHandler(HttpContext context)
    {
        var ipAsString = context.RetrieveFromUri(IpId);

        IPMemory.Check(ipAsString, out object? result);

        string info = result is IPQueryInfo ipInfo 
            ? ipInfo.ToJsonString() 
            : string.Empty;

        await context.WriteToBodyAsync(info);
    }

    public static async Task CacheHandler(HttpContext context)
    {
        var ipAsString = context.RetrieveFromUri(IpId);
        
        var value = context.ReadJsonAsync<IPQueryInfo>();
        IPMemory.Set(ipAsString, value);

        await context.WriteToBodyAsync(DateTime.Now);
    }
}

