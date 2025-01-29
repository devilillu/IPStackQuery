using Helper;
using IPStack;

namespace Service.IPBatch;

public class Program
{
    public static void Main(string[] args)
    {
        var processor = BatchService.IPBatch;

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        var app = builder.Build();
        app.UseAuthorization();

        app.MapGet("/", (HttpContext context) => context.WriteToBodyAsync("IPBatch"));
        app.MapGet(CacheQueryCom.SetPortPattern, CacheQueryCom.SetPortHandler);        
        app.MapPost(BatchService.NewBatchPattern, BatchService.BatchProcess);
        app.MapGet(BatchService.QueryStatusPattern, BatchService.QueryStatus);

        app.Run();
    }
}
