namespace Service.IPBatch;

public record class IPBatch(Guid GUID, List<IPBatchItem> Contracts);

public static class MoreIPBatch
{
    public static float CompletePercent(this IPBatch batch) =>
        100 * (float)batch.Contracts.Where(c => c.Status).Count() / batch.Contracts.Count;
}