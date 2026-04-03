using AvansDevOps.Domain;
using AvansDevOps.Domain.States;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for the BacklogItem state transitions (State Pattern).
/// Uses path coverage for the complex state machine logic.
/// </summary>
public class BacklogItemStateTests
{
    private BacklogItem CreateItem() => new("Test Item", "Description", 5);

    // === Happy path: Todo -> Doing -> ReadyForTesting -> Testing -> Tested -> Done ===

    [Fact]
    public void NewBacklogItem_ShouldBeInTodoState()
    {
        var item = CreateItem();
        Assert.Equal("Todo", item.GetStateName());
    }

    [Fact]
    public void TodoItem_MoveToDoing_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        Assert.Equal("Doing", item.GetStateName());
    }

    [Fact]
    public void DoingItem_MoveToReadyForTesting_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        Assert.Equal("ReadyForTesting", item.GetStateName());
    }

    [Fact]
    public void ReadyForTestingItem_MoveToTesting_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        Assert.Equal("Testing", item.GetStateName());
    }

    [Fact]
    public void TestingItem_MoveToTested_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        Assert.Equal("Tested", item.GetStateName());
    }

    [Fact]
    public void TestedItem_MoveToDone_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToDone();
        Assert.Equal("Done", item.GetStateName());
    }

    // === Rejection paths ===

    [Fact]
    public void ReadyForTestingItem_MoveToTodo_TesterRejects_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTodo(); // Tester finds issues
        Assert.Equal("Todo", item.GetStateName());
    }

    [Fact]
    public void TestingItem_MoveToTodo_TestFails_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTodo(); // Test failed
        Assert.Equal("Todo", item.GetStateName());
    }

    [Fact]
    public void TestedItem_MoveToReadyForTesting_DoD_NotMet_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToReadyForTesting(); // DoD not met
        Assert.Equal("ReadyForTesting", item.GetStateName());
    }

    [Fact]
    public void DoneItem_MoveToTodo_Regression_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToDone();
        item.MoveToTodo(); // Regression found
        Assert.Equal("Todo", item.GetStateName());
    }

    // === Invalid transitions ===

    [Fact]
    public void TodoItem_MoveToReadyForTesting_ShouldThrow()
    {
        var item = CreateItem();
        Assert.Throws<InvalidOperationException>(() => item.MoveToReadyForTesting());
    }

    [Fact]
    public void TodoItem_MoveToDone_ShouldThrow()
    {
        var item = CreateItem();
        Assert.Throws<InvalidOperationException>(() => item.MoveToDone());
    }

    [Fact]
    public void DoingItem_MoveToTesting_ShouldThrow()
    {
        var item = CreateItem();
        item.MoveToDoing();
        Assert.Throws<InvalidOperationException>(() => item.MoveToTesting());
    }

    [Fact]
    public void ReadyForTestingItem_MoveToDoing_ShouldThrow()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        Assert.Throws<InvalidOperationException>(() => item.MoveToDoing());
    }

    [Fact]
    public void DoneItem_MoveToDoing_ShouldThrow()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToDone();
        Assert.Throws<InvalidOperationException>(() => item.MoveToDoing());
    }

    // === Business rule: All activities must be done before backlog item can be done ===

    [Fact]
    public void MoveToDone_WithIncompleteActivities_ShouldThrow()
    {
        var item = CreateItem();
        item.AddActivity(new Activity("Task 1", "Do something"));
        item.AddActivity(new Activity("Task 2", "Do something else"));

        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();

        Assert.Throws<InvalidOperationException>(() => item.MoveToDone());
    }

    [Fact]
    public void MoveToDone_WithAllActivitiesComplete_ShouldSucceed()
    {
        var item = CreateItem();
        var act1 = new Activity("Task 1", "Do something");
        var act2 = new Activity("Task 2", "Do something else");
        act1.MarkDone();
        act2.MarkDone();
        item.AddActivity(act1);
        item.AddActivity(act2);

        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToDone();

        Assert.Equal("Done", item.GetStateName());
    }

    [Fact]
    public void MoveToDone_WithPartialActivitiesComplete_ShouldThrow()
    {
        var item = CreateItem();
        var act1 = new Activity("Task 1", "Do something");
        var act2 = new Activity("Task 2", "Do something else");
        act1.MarkDone();
        // act2 not done
        item.AddActivity(act1);
        item.AddActivity(act2);

        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();

        Assert.Throws<InvalidOperationException>(() => item.MoveToDone());
    }

    // === Full round-trip: rejection and re-completion ===

    [Fact]
    public void FullRoundTrip_RejectAndRecomplete_ShouldSucceed()
    {
        var item = CreateItem();

        // First attempt
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTodo(); // Test failed

        // Second attempt
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToDone();

        Assert.Equal("Done", item.GetStateName());
    }

    [Fact]
    public void TestedToReadyForTesting_ThenRejectedToTodo_ShouldSucceed()
    {
        var item = CreateItem();
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToReadyForTesting(); // DoD not met, back to testing
        item.MoveToTodo(); // Tester also rejects

        Assert.Equal("Todo", item.GetStateName());
    }
}
