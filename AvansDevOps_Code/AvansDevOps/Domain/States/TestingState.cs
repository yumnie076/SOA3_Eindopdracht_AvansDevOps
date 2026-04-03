namespace AvansDevOps.Domain.States;

// STATE PATTERN: Concrete state for 'Testing' phase.
// From Testing, item can move to Tested or back to Todo (if test fails).
public class TestingState : IBacklogItemState
{
    public string Name => "Testing";

    public void MoveToTodo(BacklogItem item)
    {
        // Test failed - item goes back to Todo
        item.SetState(new TodoState());
        item.NotifySubscribers($"Backlog item '{item.Title}' failed testing and moved back to Todo.");
    }

    public void MoveToDoing(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Testing to Doing.");
    }

    public void MoveToReadyForTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Testing back to ReadyForTesting.");
    }

    public void MoveToTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Backlog item is already in Testing.");
    }

    public void MoveToTested(BacklogItem item)
    {
        item.SetState(new TestedState());
    }

    public void MoveToDone(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Testing to Done.");
    }
}
