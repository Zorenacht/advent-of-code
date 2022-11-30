using Tools;

Stopwatch.Time(part2);

void part1()
{
    int[] playerPos = new int[2] { 4, 3 };
    int[] playerCount = new int[2] { 0, 0 };

    int detDie = 1;
    int turn = 0;
    while (playerCount.Max() < 1000)
    {
        playerPos[turn] = (playerPos[turn] - 1 + 3 * detDie + 3) % 10 + 1;
        playerCount[turn] += playerPos[turn];
        detDie += 3;
        turn = (turn + 1) % 2;
    }
    Console.WriteLine(detDie - 1 + " " + string.Join(' ', playerCount));
    Console.WriteLine((detDie - 1) * playerCount.Min());
}
void part2()
{
    Game game = new Game();
    List<(int, int)> state = new List<(int, int)>() { (4,0), (3,0) };
    game.Run(1, state, 0);

    Console.WriteLine("Numer of dimensions: " + string.Join(",", game.count));

}

public class Game
{
    public Dictionary<int, int> distribution = new Dictionary<int, int>() { { 3, 1 }, { 4, 3 }, { 5, 6 }, { 6, 7 }, { 7, 6 }, { 8, 3 }, { 9, 1 } };
    public long[] count = new long[2];

    public void Run(long branchCount, List<(int, int)> state, int player)
    {
        foreach (KeyValuePair<int, int> dist in distribution)
        {
            var newPlayerState = NewPlayerState(player, dist.Key, state);
            var newState = new List<(int, int)>() 
            { 
                player == 0 ? newPlayerState : state[0], 
                player == 1 ? newPlayerState : state[1]
            };
            if( newState[player].Item2 >= 21)
            {
                count[player] += dist.Value * branchCount;
            }
            else
            {
                Run(dist.Value * branchCount, newState , (player+1) % 2);
            }
        }
    }

    private (int,int) NewPlayerState(int player, int roll, List<(int,int)> currentState)
        => (NewPos(currentState[player].Item1, roll), currentState[player].Item2 + NewPos(currentState[player].Item1, roll));
    
    private int NewPos(int pos, int roll) 
        => (pos + roll - 1) % 10 + 1;
}