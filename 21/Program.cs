using System.Text;
const int part2maxCount = 21;

//part1();
part2();

void part2()
{
    long[,,,] dynamicArray = new long[10, 10, part2maxCount + 1, part2maxCount + 1]; //p1,p2,total count
    dynamicArray[4 - 1, 8 - 1, 0, 0] = 1;
    for (int count = 0; count < dynamicArray.GetLength(2); count++)
    {
        print(dynamicArray, count);
        for (int p1 = 0; p1 < dynamicArray.GetLength(0); p1++)
        {
            for (int p2 = 0; p2 < dynamicArray.GetLength(1); p2++)
            {
                diceRolls(count, p1, p2, dynamicArray);
            }
        }
    }
    print(dynamicArray, part2maxCount);

    int p1count = 0;
    int p2count = 0;
    for (int p1 = 0; p1 < dynamicArray.GetLength(0); p1++)
    {
        for (int p2 = 0; p2 < dynamicArray.GetLength(1); p2++)
        {
            
        }
    }
}

void diceRolls(int count, int p1, int p2, long[,,,] dynamicArray)
{
    for(int i=1; i <= 3; i++)
    {
        for (int j = 1; j <= 3; j++)
        {
            for (int k = 1; k <= 3; k++)
            {
                int roll = i + j + k;

                int newP1pos = newPos(p1, roll);
                int newP1count = Math.Min(count + newP1pos, part2maxCount);
                dynamicArray[newP1pos - 1, p2, newP1count] += dynamicArray[p1, p2, count];

                int newP2pos = newPos(p2, roll);
                int newP2count = Math.Min(count + newP2pos, part2maxCount);
                dynamicArray[p1, newP2pos - 1, newP2count] += dynamicArray[p1, p2, count];
            }
        }
    }
}

int newPos(int player, int roll)
{
    return (player + roll - 1) % 10 + 1;
}

void print(long[,,] dynamicArray, int count)
{
    StringBuilder sb = new();
    for (int p1 = 0; p1 < dynamicArray.GetLength(0); p1++)
    {
        for (int p2 = 0; p2 < dynamicArray.GetLength(1); p2++)
        {
            string val = dynamicArray[p1, p2, count].ToString();
            sb.Append(dynamicArray[p1, p2, count] + new string(' ', 20-val.Length) + ",");
        }
        sb.Append('\n');
    }
    Console.WriteLine(sb.ToString());
}

void part1(){
    int[] playerPos = new int[2] { 4, 3 };
    int[] playerCount = new int[2] { 0, 0 };

    int detDie = 1;
    int turn = 0;
    while(playerCount.Max() < 1000)
    {
        playerPos[turn] = (playerPos[turn]-1 + 3 * detDie + 3) % 10 + 1;
        playerCount[turn] += playerPos[turn];
        detDie += 3;
        turn = (turn + 1) % 2;
    }
    Console.WriteLine(detDie-1 + " " + string.Join(' ', playerCount));
    Console.WriteLine((detDie-1) * playerCount.Min());
}
