namespace Tools;

public static class Reader
{
    public static string ReadAsText(string filename)
    {
        string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
        return File.ReadAllText(fileLocation);
    }

    public static string[] ReadLines(string filename)
    {
        string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
        return File.ReadAllLines(fileLocation);
    }

    public static bool TryReadAsText(string filename, out string text)
    {
        string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
        if (File.Exists(fileLocation))
        {
            text = ReadAsText(fileLocation);
            return true;
        }
        text = string.Empty;
        return false;
    }

    public static bool TryReadLines(string filename, out string[] lines)
    {
        string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"../../../", filename);
        if (File.Exists(fileLocation))
        {
            lines = ReadLines(fileLocation);
            return true;
        }
        lines = Array.Empty<string>();
        return false;
    }
}
