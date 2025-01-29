using Helper;

namespace Service.IPCache;

public class Program
{
    public static void Main(string[] args)
    {
        var mem = CacheService.IPMemory;

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();

        var app = builder.Build();
        app.UseAuthorization();

        app.MapGet("/", (HttpContext context) => context.WriteToBodyAsync("IPCache"));
        app.MapGet(CacheService.IpCheckPattern, CacheService.CheckHandler);
        app.MapPost(CacheService.IpCachePattern, CacheService.CacheHandler);

        app.Run();
    }
}