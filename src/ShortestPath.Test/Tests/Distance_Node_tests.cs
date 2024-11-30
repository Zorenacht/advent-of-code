namespace ShortestPath.Test.Tests;

public class Distance_Node_tests
{
    [Test]
    public void Generates_correct_next_distance_nodes()
    {
        var state = new SimpleState { Pos = 1 };
        var distanceNode = new Node<SimpleState>(state, 1000);
        distanceNode.NextNodes().Should().BeEquivalentTo(new List<Node<SimpleState>>
        {
            new(new SimpleState { Pos = 2 }, 1001),
            new(new SimpleState { Pos = 3 }, 1010),
            new(new SimpleState { Pos = 4 }, 1100),
        });
    }

    [Test]
    public void Generates_correct_next_distance_nodes_for_amphipods1()
    {
        var hallway = Enumerable.Repeat(Amphipod.O, 11).ToArray();
        var initialRooms = new Amphipod[2, 4]
        {
           { Amphipod.B, Amphipod.B, Amphipod.C, Amphipod.D },
           { Amphipod.D, Amphipod.A, Amphipod.A, Amphipod.C },
        };
        var state = new AmphipodState(hallway, initialRooms);

        var distanceNode = new Node<AmphipodState>(state, 0);
        var nextNodes = distanceNode.NextNodes();
        nextNodes.Count().Should().Be(28);
    }



    [Test]
    public void SpaceInRoom()
    {
        var hallway = Enumerable.Repeat(Amphipod.O, 11).ToArray();
        var initialRooms = new Amphipod[2, 4]
        {
           { Amphipod.B, Amphipod.B, Amphipod.C, Amphipod.A },
           { Amphipod.D, Amphipod.A, Amphipod.C, Amphipod.D },
        };
        var state = new AmphipodState(hallway, initialRooms);
        state.SpaceInRoom(Amphipod.A).Should().Be(2);
        state.SpaceInRoom(Amphipod.B).Should().Be(2);
        state.SpaceInRoom(Amphipod.C).Should().Be(0);
        state.SpaceInRoom(Amphipod.D).Should().Be(1);
    }
}