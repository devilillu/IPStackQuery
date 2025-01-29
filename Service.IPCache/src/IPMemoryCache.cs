using Microsoft.Extensions.Caching.Memory;

namespace Service.IPCache;

internal class IPMemoryCache
{
    internal IPMemoryCache() => _cache = new(new MemoryCacheOptions());

    internal bool Check(string key, out object? result) => _cache.TryGetValue(key, out result);

    internal void Set(string key, object value) => _cache.Set(key, value, TimeSpan.FromMinutes(1));

    internal readonly MemoryCache _cache;
}

