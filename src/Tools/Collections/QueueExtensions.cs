namespace Collections;

public static class QueueExtensions
{
    public static Queue<T> With<T>(this Queue<T> queue, params T[] range)
    {
        foreach (var element in range)
            queue.Enqueue(element);
        return queue;
    }

    public static PriorityQueue<T, TPriority> With<T, TPriority>(this PriorityQueue<T, TPriority> queue, T element, TPriority priority)
    {
        queue.Enqueue(element, priority);
        return queue;
    }
}