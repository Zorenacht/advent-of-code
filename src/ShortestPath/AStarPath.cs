namespace ShortestPath;

public class AStarPath<T> : IShortestPath<T> where T : IState<T>
{
    public int ShortestPath => Succesful && CurrentNode is not null ? CurrentNode.Distance : -1;
    public int Iterations { get; set; }

    private T Start { get; init; }
    private T Goal { get; init; }
    private HashSet<Node<T>> Visited { get; set; }
    private PriorityQueue<Node<T>, int> NotVisited { get; set; }
    private Node<T>? CurrentNode { get; set; }
    private bool Succesful { get; set; }


    private Func<T, bool> Done { get; set; }

    public AStarPath(T start, T goal)
    {
        Start = start;
        Goal = goal;
        Iterations = 0;

        var node = new Node<T>(Start, 0);
        CurrentNode = node;

        Visited = new HashSet<Node<T>>(new NodeComparer<T>());
        NotVisited = new PriorityQueue<Node<T>, int>();

        Done = state => state.Equals(Goal);
    }

    public AStarPath(T start, T goal, Func<T, bool> done) : this(start, goal)
    {
        Done = done;
    }

    public void Run()
    {
        AStar();
    }

    public void AStar()
    {
        while (CurrentNode is not null)
        {
            Iterations++;
            if (ReachedGoal())
            {
                Succesful = true;
                return;
            }
            AddNextNodes();
            MoveToNextNode();
        }
    }

    private bool ReachedGoal() => CurrentNode is not null ? Done(CurrentNode.State) : false;

    private void AddNextNodes()
    {
        if (CurrentNode is null) return;
        foreach (var newNode in CurrentNode.NextNodes())
        {
            if (Visited.Contains(newNode))
                continue;
            NotVisited.Enqueue(newNode, newNode.Distance + newNode.Heuristic);
        }
    }

    private void MoveToNextNode()
    {
        while (NotVisited.Count > 0)
        {
            var minNode = NotVisited.Dequeue();
            if (Visited.Contains(minNode))
                continue;
            Visited.Add(minNode);
            CurrentNode = minNode;
            return;
        }
        CurrentNode = null;
    }

    private void PrintResult()
    {
        if (Succesful)
        {
            Console.WriteLine("Shortest path is: " + "Not yet implemented.");
            Console.WriteLine($"Shortest path value: {ShortestPath}");
            Console.WriteLine($"Number of iterations: {Iterations}");
            Console.WriteLine(CurrentNode!.State);
        }
        else
        {
            Console.WriteLine("Did not find shortest path, something went wrong.");
        }
    }
}