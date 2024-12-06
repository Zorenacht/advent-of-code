using System.Collections;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day10 : Day
{
    [Puzzle(answer: 80)]
    public int Part1Example() => new Loop(InputExample).Length / 2;
    
    [Puzzle(answer: 6717)]
    public int Part1() => new Loop(Input).Length / 2;
    
    [Puzzle(answer: 10)]
    public int Part2Example() => new Loop(InputExample).Inner();
    
    [Puzzle(answer: 381)]
    public int Part2() => new Loop(Input).Inner();
    
    private class Loop
    {
        private readonly Grid<char> _board;
        private readonly HashSet<Index2D> _cycle;
        private IndexDirection Start { get; set; }
        
        public Loop(string[] board)
        {
            _board = board.AddBorder('*').ToGrid();
            var start = _board.FindIndexes('S').First();
            var dir = StartDirection(_board, start);
            _cycle = CyclePoints(_board, start, dir);
            Start = new IndexDirection(start, dir);
        }
        
        public int Length => _cycle.Count;
        
        public int Inner(bool print = false)
        {
            var start = Start.Index;
            var left = new HashSet<Index2D>();
            var right = new HashSet<Index2D>();
            var iterator = new IndexDirection(
                start.Neighbors(Start.Direction),
                (Direction)(((int)Start.Direction + 4) % 8));
            do
            {
                var p = iterator.Index;
                var dir = iterator.Direction;
                var lef = Left(p, dir, _board[p.Row][p.Col]);
                var rig = Right(p, dir, _board[p.Row][p.Col]);
                foreach (var l in lef)
                {
                    if (_board[l.Row][l.Col] != '*' && !_cycle.Contains(l) && !left.Contains(l) && !right.Contains(l))
                    {
                        left.Add(l);
                    }
                }
                foreach (var r in rig)
                {
                    if (_board[r.Row][r.Col] != '*' && !_cycle.Contains(r) && !left.Contains(r) && !right.Contains(r))
                    {
                        right.Add(r);
                    }
                }
                iterator = Mapping(_board[p.Row][p.Col], iterator).First();
                _cycle.Add(iterator.Index);
            } while (iterator.Index != start);
            
            FloodFill(_board, left, right, _cycle);
            
            if (print) PrintColor(_board, [(_cycle, ConsoleColor.Blue), (left, ConsoleColor.Red), (right, ConsoleColor.Green)]);
            
            return Math.Min(left.Count, right.Count);
        }
        
        
        public IEnumerable<Index2D> Left(Index2D p, Direction incoming, char ch)
        {
            var left = (Direction)(((int)incoming + 2) % 8);
            yield return p.Neighbors(left);
            var nextIncomingDir = Mapping(ch, new IndexDirection(p, incoming)).First().Direction;
            var diff = ((int)nextIncomingDir - (int)incoming + 8) % 8;
            var straight = (Direction)(((int)incoming + 4) % 8);
            if (diff == 2) yield return p.Neighbors(straight);
        }
        
        public IEnumerable<Index2D> Right(Index2D p, Direction incoming, char ch)
        {
            var right = (Direction)(((int)incoming + 6) % 8);
            yield return p.Neighbors(right);
            var nextIncomingDir = Mapping(ch, new IndexDirection(p, incoming)).First().Direction;
            var diff = ((int)nextIncomingDir - (int)incoming + 8) % 8;
            var straight = (Direction)(((int)incoming + 4) % 8);
            if (diff == 6) yield return p.Neighbors(straight);
        }
        
        public record IndexDirection(Index2D Index, Direction Direction);
        
        private Direction StartDirection(Grid<char> board, Index2D start)
        {
            IndexDirection[] nb =
            [
                new IndexDirection(start.Neighbors(Direction.W), Direction.E),
                new IndexDirection(start.Neighbors(Direction.N), Direction.S),
                new IndexDirection(start.Neighbors(Direction.E), Direction.W),
                new IndexDirection(start.Neighbors(Direction.S), Direction.N)
            ];
            var incomingDirection = nb.First(pd => Mapping(board[pd.Index.Row][pd.Index.Col], pd).Any())
                .Direction;
            return incomingDirection.Backwards();
        }
        
        public void FloodFill(Grid<char> board, HashSet<Index2D> left, HashSet<Index2D> right, HashSet<Index2D> cycle)
        {
            for (int i = 0; i < board.RowLength; i++)
            {
                for (int j = 0; j < board.ColLength; j++)
                {
                    var p = new Index2D(i, j);
                    bool? foundLeft = null;
                    if (board[p.Row][p.Col] != '*' && !cycle.Contains(p) && !left.Contains(p) && !right.Contains(p))
                    {
                        var all = new HashSet<Index2D>() { };
                        var currs = new List<Index2D>() { p };
                        while (currs.Count > 0)
                        {
                            var next = new List<Index2D>();
                            foreach (var curr in currs)
                            {
                                if (all.Contains(curr) || cycle.Contains(curr) || board[curr.Row][curr.Col] == '*') continue;
                                next.AddRange(Neighbors(curr).Where(poi => !cycle.Contains(poi) && board[poi.Row][poi.Col] != '*'));
                                all.Add(curr);
                            }
                            currs = next;
                        }
                        if (all.Overlaps(left)) foundLeft = true;
                        if (all.Overlaps(right)) foundLeft = false;
                        all.UnionWith(all);
                        if (foundLeft is null) throw new Exception();
                        else if (foundLeft.Value) left.UnionWith(all);
                        else if (!foundLeft.Value) right.UnionWith(all);
                    }
                }
            }
        }
        
        public static HashSet<Index2D> CyclePoints(Grid<char> board, Index2D start, Direction direction)
        {
            var iterator = new IndexDirection(start.Neighbors(direction), direction.Backwards());
            var cycle = new HashSet<Index2D>();
            do
            {
                var p = iterator.Index;
                cycle.Add(iterator.Index);
                iterator = Mapping(board[p.Row][p.Col], iterator).First();
            } while (iterator.Index != start);
            cycle.Add(iterator.Index);
            return cycle;
        }
        
        public static Index2D[] Neighbors(Index2D p)
            =>
            [
                new Index2D(p.Row, p.Col - 1),
                new Index2D(p.Row, p.Col + 1),
                new Index2D(p.Row - 1, p.Col),
                new Index2D(p.Row + 1, p.Col)
            ];
        
        private static IEnumerable<IndexDirection> Mapping(char ch, IndexDirection indexDir)
        {
            return (ch, indexDir.Direction) switch
            {
                ('|', Direction.S) => [new IndexDirection(indexDir.Index.Neighbors(Direction.N), Direction.S)],
                ('|', Direction.N) => [new IndexDirection(indexDir.Index.Neighbors(Direction.S), Direction.N)],
                ('-', Direction.W) => [new IndexDirection(indexDir.Index.Neighbors(Direction.E), Direction.W)],
                ('-', Direction.E) => [new IndexDirection(indexDir.Index.Neighbors(Direction.W), Direction.E)],
                ('L', Direction.N) => [new IndexDirection(indexDir.Index.Neighbors(Direction.E), Direction.W)],
                ('L', Direction.E) => [new IndexDirection(indexDir.Index.Neighbors(Direction.N), Direction.S)],
                ('J', Direction.N) => [new IndexDirection(indexDir.Index.Neighbors(Direction.W), Direction.E)],
                ('J', Direction.W) => [new IndexDirection(indexDir.Index.Neighbors(Direction.N), Direction.S)],
                ('7', Direction.S) => [new IndexDirection(indexDir.Index.Neighbors(Direction.W), Direction.E)],
                ('7', Direction.W) => [new IndexDirection(indexDir.Index.Neighbors(Direction.S), Direction.N)],
                ('F', Direction.S) => [new IndexDirection(indexDir.Index.Neighbors(Direction.E), Direction.W)],
                ('F', Direction.E) => [new IndexDirection(indexDir.Index.Neighbors(Direction.S), Direction.N)],
                ('S', _) =>
                [
                    new IndexDirection(indexDir.Index.Neighbors(Direction.W), Direction.E),
                    new IndexDirection(indexDir.Index.Neighbors(Direction.N), Direction.S),
                    new IndexDirection(indexDir.Index.Neighbors(Direction.E), Direction.W),
                    new IndexDirection(indexDir.Index.Neighbors(Direction.S), Direction.N)
                ],
                _ => []
            };
        }
        
        private static void PrintColor(Grid<char> board, params (HashSet<Index2D> Set, ConsoleColor Color)[] highlight)
        {
            for (int i = 0; i < board.RowLength; i++)
            {
                for (int j = 0; j < board.ColLength; j++)
                {
                    if (highlight.Any(x => x.Set.Contains(new Index2D(i, j))))
                    {
                        foreach (var h in highlight)
                        {
                            if (h.Set.Contains(new Index2D(i, j)))
                            {
                                Console.BackgroundColor = h.Color;
                                Console.Write(board[i][j]);
                                Console.ResetColor();
                                break;
                            }
                        }
                    }
                    else if (board[i][j] == '.')
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(board[i][j]);
                        Console.ResetColor();
                    }
                    else if (board[i][j] == '*')
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(board[i][j]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(board[i][j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}