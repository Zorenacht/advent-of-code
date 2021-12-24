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
}
