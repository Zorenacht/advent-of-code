using AoC._2021.Models;

namespace ShortestPath.Test.Tests;

public class Day23 : Day
{ 
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
    public void Part1(Amphipod[,] initialRooms, int expected)
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
    public void Part2(Amphipod[,] initialRooms, int expected)
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