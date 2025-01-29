using Helper;
using IPStack;

namespace Service.IPQuery;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        var app = builder.Build();
        app.UseAuthorization();

        QueryService.APIKey = builder.Configuration.GetValue<string>("APIKey");

        app.MapGet("/", (HttpContext context) => context.WriteToBodyAsync("IPQuery"));
        app.MapGet(CacheQueryCom.SetPortPattern, CacheQueryCom.SetPortHandler);
        app.MapGet(QueryService.IpQueryPattern, QueryService.Handler);

        app.Run();
    }
}
