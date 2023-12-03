using System.Text;

namespace Tools;

public static class CharExtensions
{
    public static bool IsDigit(this char ch)
        => char.IsDigit(ch);

    public static int ToInt(this char ch)
        => char.IsDigit(ch) 
            ? ch - '0' 
            : throw new NotSupportedException($"Character {ch} is not a digit.");
}
