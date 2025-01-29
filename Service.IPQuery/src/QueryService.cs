using Helper;
using IPQuery;
using IPStack;

namespace Service.IPQuery;

public static class QueryService
{
    internal static string? APIKey;

    public static string IpQueryPattern => "/api/query/{" + IpId + "}";

    static string IpId => "ip";

    public static async Task Handler(HttpContext context)
    {
        var ipQuery = new IPQuerySeed(context.RetrieveFromUri(IpId));
        IPQueryResult result;

        string cachedValue = await CacheQueryCom.IsAvailble(ipQuery);

        if (string.IsNullOrWhiteSpace(cachedValue))
        {
            //TODO
            result = APIKey == null
                ? await QueryCom.QueryDev(ipQuery)
                : await QueryCom.QueryIPStack(ipQuery, APIKey);

            CacheQueryCom.UpdateCache(result);
        }
        else
        {
            result = new(ipQuery, new(DateTime.Now, cachedValue));
        }

        await context.WriteToBodyAsync(result.ToJsonString());
    }
}
