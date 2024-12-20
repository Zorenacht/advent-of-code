using AoC._2024;
using System.Diagnostics;


var day = new Day20();
ElapsedTime(() =>
{
    for (int i = 0; i < 1; ++i)
    {
        var result = day.Part2();
    }
    //Console.WriteLine($"Answer: {result}");
});


void ElapsedTime(Action action)
{
    var sw = Stopwatch.StartNew();
    Console.WriteLine(sw.ElapsedMilliseconds);
    action();
    Console.WriteLine(sw.ElapsedMilliseconds);
}