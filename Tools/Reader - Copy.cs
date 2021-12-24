namespace Tools;

public static class Stopwatch
{
    public static void Time(Action action)
    {
        DateTime startTime, endTime;
        startTime = DateTime.Now;

        action();

        endTime = DateTime.Now;
        double elapsedMillisecs = ((TimeSpan)(endTime - startTime)).TotalMilliseconds;
        Console.WriteLine("Ms: " + elapsedMillisecs);
    }
}
