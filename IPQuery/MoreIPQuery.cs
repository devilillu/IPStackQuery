using System.Text.Json;

namespace IPQuery;

public static class MoreIPQuery
{
    public static string ToJsonString(this IPQueryResult result) => JsonSerializer.Serialize(result);

    public static string ToJsonString(this IPQueryInfo info) => JsonSerializer.Serialize(info);
}
