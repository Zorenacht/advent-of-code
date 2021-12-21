/*using Tools;
using Tools.Geometry;

part1();

void part1()
{
    init("Input.txt", out string coding, out Grid grid);
    grid.Print(); 
    Console.WriteLine("Iteration: " + 0 + " Count: " + grid.Count());
    for (int i = 1; i <= 2; i++)
    {
        grid = run(grid, coding, i);
        grid.Print();
        Console.WriteLine("Iteration: " + i + " Count: " + grid.Count());
    }
}

Grid run(Grid grid, string coding, int iteration)
{
    Grid nextIteration = new Grid(grid.RowLength, grid.ColLength);
    bool border = borderVal(coding, iteration);
    for (int i = 0; i < grid.RowLength; i++)
    {
        for (int j = 0; j < grid.ColLength; j++)
        {
            //Console.WriteLine(grid.ColLength + "i,j: " + i + ", " + j);
            nextIteration.Rows[i][j] = findCoding(grid, coding, i, j, border);
        }
    }
    nextIteration.ExtendBorder(border);
    return nextIteration;
}

bool borderVal(string coding, int iteration)
{
    if (coding[0] == '.') return false;
    if (coding[^1] == '.') return iteration % 2 == 1;
    return iteration > 0;
}

bool findCoding(Grid grid, string coding, int i, int j, bool border)
{
    Point point = new Point(i, j);
    //Console.WriteLine("Border: " + border + " Iteration: " + iteration);
    int code = 0;
    code = 2 * code + (grid.Value(point.Neighbor(Direction.NW), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.N), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.NE), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.W), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point, border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.E), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.SW), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.S), border) ? 1 : 0);
    code = 2 * code + (grid.Value(point.Neighbor(Direction.SE), border) ? 1 : 0);
    //Console.WriteLine(code);
    return coding[code] == '#' ? true : false;
}

void init(string input, out string coding, out Grid grid)
{
    List<string> lines = Reader.ReadLines(input).ToList();
    coding = lines[0];

    grid = new Grid();
    foreach (var line in lines.GetRange(2, lines.Count-2)) 
    {
        grid.Rows.Add(Parser(line));
    }
    grid.ExtendBorder(borderVal(coding, 0));
}

List<bool> Parser(string input)
{
    List<bool> row = new List<bool>();
    foreach (char c in input)
    {
        if (c == '#')
            row.Add(true);
        else if (c == '.')
            row.Add(false);
    }
    return row;
}

public class Grid
{
    public List<List<bool>> Rows = new();

    public int RowLength => Rows.Count;
    public int ColLength => Rows.FirstOrDefault()?.Count ?? 0;

    public Grid()
    {

    }

    public Grid(int row, int col)
    {
        for(int i = 0; i < row; i++)
        {
            Rows.Add(new List<bool>(new bool[col]));
        }
    }

    public void Print()
    {
        Console.WriteLine(new string('-', ColLength));
        foreach(var row in Rows)
        {
            foreach(var val in row)
            {
                Console.Write((val ? "#" : "."));
            }
            Console.WriteLine();
        }
    }

    public bool Value(Point point, bool border)
    {
        return point.X >= 0 && point.Y >= 0 && point.X < RowLength && point.Y < ColLength
            ? Rows[point.X][point.Y]
            : border;
    }

    public int Count()
    {
        int count = 0;
        foreach(var row in Rows)
        {
            foreach(var val in row)
            {
                if(val) count++;
            }
        }
        return count;
    }

    public void ExtendBorder(bool border)
    {
        bool wBorderNotEmpty = false;
        bool eBorderNotEmpty = false;
        bool nBorderNotEmpty = false;
        bool sBorderNotEmpty = false;
        foreach (List<bool> row in Rows)
        {
            if (row[0])  wBorderNotEmpty = true;
            if (row[^1]) eBorderNotEmpty = true;
        }
        foreach (bool val in Rows[0]){
            if(val)  nBorderNotEmpty = true;
        }
        foreach (bool val in Rows[^1])
        {
            if (val) sBorderNotEmpty = true;
        }
        if (nBorderNotEmpty)
        {
            Rows.Insert(0, new List<bool>(Enumerable.Repeat(border, ColLength).ToArray()));
        }
        if (sBorderNotEmpty)
        {
            Rows.Add(new List<bool>(Enumerable.Repeat(border, ColLength).ToArray()));
        }
        if (wBorderNotEmpty)
        {
            foreach(var row in Rows)
            {
                row.Insert(0, border);
            }
        }
        if (eBorderNotEmpty)
        {
            foreach (var row in Rows)
            {
                row.Add(border);
            }
        }
    }
}*/