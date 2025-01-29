namespace Service.IPBatch;

public class IPBatchItem
{
    public IPBatchItem(string ip)
    {
        Status = false;
        Ip = ip;
    }

    public string Ip { get; init; }

    public bool Status { get; private set; }

    public void SetStatus() => Status = true;
}