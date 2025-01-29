using Helper;

namespace Service.IPBatch;

public static class BatchService
{
    public static IPBatchProcess IPBatch = new();

    public static string NewBatchPattern => "/api/batch";

    public static string QueryStatusPattern => "/api/query/{" + BatchGuid + "}";

    static string BatchGuid => "GUID";

    public static async Task BatchProcess(HttpContext context) 
    {
        var ipLookupContracts = await context.ReadJsonAsync<List<string>>() ?? 
            throw new BadHttpRequestException("Invalid list format...");

        var guid = await Task.FromResult(IPBatch.Queue(ipLookupContracts));
        await context.WriteToBodyAsync($"Batch now in queue GUID:{guid}");
    }

    public static async Task QueryStatus(HttpContext context)
    {
        var guid = new Guid(context.RetrieveFromUri(BatchGuid));
        var batchStatus = await Task.FromResult(IPBatch.QueryStatus(guid));
        await context.WriteToBodyAsync($"Batch status: {batchStatus.PercentComplete}%");
    }

}

