using Tools.Geometry;

namespace _11.Tools;

internal class FlashingOctopusGrid : Grid
{
    public FlashingOctopusGrid(string text) : base(text) { }

    public void Print(int step, int count)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"After step {step}:");
        foreach (int[] row in Lattice)
        {
            foreach (int element in row)
            {
                if (element == 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Count: " + count);
    }
}
