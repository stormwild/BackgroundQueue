using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace BackgroundQueue.Api;

public class BackgroundQueue<T> : IBackgroundQueue<T> where T : class
{
    private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();

    public void Enqueue(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Enqueue(item);
    }

    public T Dequeue()
    {
        var success = _items.TryDequeue(out var workItem);

        return success
            ? workItem
            : null;
    }
}
