namespace ShortestPath.Test.Tests;

public class AStar_tests
{
    [Test]
    public void Shortest_path_simple_model()
    {
        var start = new SimpleState() { Pos = 1 };
        var end = new SimpleState() { Pos = 10 };
        var a = new AStarPath<SimpleState>(start, end);
        a.Run();
        a.ShortestPath.Should().Be(9);
    }

    public static IEnumerable<TestCaseData> InitialRooms_Day1()
    {
        yield return new TestCaseData(new Amphipod[2, 4]
        {
            { Amphipod.C, Amphipod.B, Amphipod.A, Amphipod.D },
            { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D },
        }, 608);
        yield return new TestCaseData(new Amphipod[2, 4]
        {
            { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D },
            { Amphipod.C, Amphipod.B, Amphipod.A, Amphipod.D },
        }, 1214);
        yield return new TestCaseData(new Amphipod[2, 4]
        {
            { Amphipod.B, Amphipod.B, Amphipod.C, Amphipod.D },
            { Amphipod.D, Amphipod.A, Amphipod.A, Amphipod.C },
        }, 15109);
    }


    [TestCaseSource(nameof(InitialRooms_Day1))]
    public void Shortest_path_amphipod_model_day_1(Amphipod[,] initialRooms, int expected)
    {
        var hallway = Enumerable.Repeat(Amphipod.O, 11).ToArray();
        var initialState = new AmphipodState(hallway, initialRooms);

        var endingRooms = new Amphipod[2, 4]
        {
           { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D },
           { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D }
        };
        var endingState = new AmphipodState(hallway, endingRooms);

        var dijkstra = new AStarPath<AmphipodState>(initialState, endingState);
        dijkstra.Run();
        dijkstra.ShortestPath.Should().Be(expected);
    }

    public static IEnumerable<TestCaseData> InitialRooms_Day2()
    {
        yield return new TestCaseData(new Amphipod[4, 4]
        {
            { Amphipod.B, Amphipod.C, Amphipod.B, Amphipod.D },
            { Amphipod.D, Amphipod.C, Amphipod.B, Amphipod.A },
            { Amphipod.D, Amphipod.B, Amphipod.A, Amphipod.C },
            { Amphipod.A, Amphipod.D, Amphipod.C, Amphipod.A },
        }, 44169);
        yield return new TestCaseData(new Amphipod[4, 4]
        {
            { Amphipod.B, Amphipod.B, Amphipod.C, Amphipod.D },
            { Amphipod.D, Amphipod.C, Amphipod.B, Amphipod.A },
            { Amphipod.D, Amphipod.B, Amphipod.A, Amphipod.C },
            { Amphipod.D, Amphipod.A, Amphipod.A, Amphipod.C },
        }, 53751);
    }


    [TestCaseSource(nameof(InitialRooms_Day2))]
    public void Shortest_path_amphipod_model_day_2(Amphipod[,] initialRooms, int expected)
    {
        var hallway = Enumerable.Repeat(Amphipod.O, 11).ToArray();
        var initialState = new AmphipodState(hallway, initialRooms);

        var endingRooms = new Amphipod[4, 4]
        {
           { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D },
           { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D },
           { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D },
           { Amphipod.A, Amphipod.B, Amphipod.C, Amphipod.D }
        };
        var endingState = new AmphipodState(hallway, endingRooms);

        var dijkstra = new AStarPath<AmphipodState>(initialState, endingState);
        dijkstra.Run();
        dijkstra.ShortestPath.Should().Be(expected);
    }
}