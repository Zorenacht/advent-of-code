using AoC._2024;
using System.Diagnostics;


var day = new Day19();
ElapsedTime(() =>
{
    var result = day.Part2();
    Console.WriteLine($"Answer: {result}");
});


void ElapsedTime(Action action)
{
    var sw = Stopwatch.StartNew();
    Console.WriteLine(sw.ElapsedMilliseconds);
    action();
    Console.WriteLine(sw.ElapsedMilliseconds);
}