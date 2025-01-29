
using IPStack;
using System.Collections.Concurrent;

namespace Service.IPBatch;

public class IPBatchProcess : IDisposable
{
    readonly AutoResetEvent _newItemEvent = new(false);
    readonly CancellationTokenSource _cts = new();
    readonly Task _processor;

    readonly ConcurrentQueue<Guid> _guidContracts = new();
    readonly ConcurrentDictionary<Guid, IPBatch> _items = new();    

    public IPBatchProcess()
    {
        _processor = new(ProcessingTask, _cts.Token);
        _processor.Start();
    }

    internal Guid Queue(List<string> contracts)
    {
        var newGuid = Guid.NewGuid();
        var items = contracts.Select(a => new IPBatchItem(a)).ToList();
        if (_items.TryAdd(newGuid, new(newGuid, items)))
            _guidContracts.Enqueue(newGuid);
        else
            throw new ApplicationException("No available Guid, better luck next time"); //add more retries

        _newItemEvent.Set();
        return newGuid;
    }

    internal IPBatchStatus QueryStatus(Guid guid)
    {
        if (!_items.TryGetValue(guid, out IPBatch? batch))
            throw new KeyNotFoundException(guid.ToString());

        return new IPBatchStatus(batch, batch.CompletePercent());
    }

    async void ProcessingTask()
    {
        try
        {
            while (true)
            {
                _newItemEvent.WaitOne();

                if (_guidContracts.TryDequeue(out var guidToProcess))
                    await Process(_items[guidToProcess]);
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {ex.Message}");
        }
    }

    async Task Process(IPBatch batch)
    {
        List<Task> tasks = new(10);
        foreach (var chunk in batch.Contracts.Chunk(10))
        {
            foreach (var contract in chunk)
               tasks.Add(ProcessContract(contract));
            await Task.WhenAll(tasks);
        }
    }

    async Task ProcessContract(IPBatchItem contract)
    {
        var result = await QueryCom.QueryDev(new(contract.Ip));
        CacheQueryCom.UpdateCache(result);
        contract.SetStatus();
    }

    private void Stop()
    {
        _cts.Cancel();
        _newItemEvent?.Set();
    }

    #region IDisposable pattern
    bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Stop();
                _cts.Dispose();
                _newItemEvent?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion IDisposable pattern
}