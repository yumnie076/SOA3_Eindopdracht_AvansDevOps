namespace AvansDevOps.Domain.States;

// STATE PATTERN: Concrete state for 'Doing' phase.
// From Doing, a backlog item can only move to ReadyForTesting.
public class DoingState : IBacklogItemState
{
    public string Name => "Doing";

    public void MoveToTodo(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Doing back to Todo.");
    }

    public void MoveToDoing(BacklogItem item)
    {
        throw new InvalidOperationException("Backlog item is already in Doing.");
    }

    public void MoveToReadyForTesting(BacklogItem item)
    {
        item.SetState(new ReadyForTestingState());
        // OBSERVER PATTERN: Notify testers that an item is ready for testing
        item.NotifySubscribers($"Backlog item '{item.Title}' is ready for testing.");
    }

    public void MoveToTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Doing to Testing.");
    }

    public void MoveToTested(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Doing to Tested.");
    }

    public void MoveToDone(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Doing to Done.");
    }
}
