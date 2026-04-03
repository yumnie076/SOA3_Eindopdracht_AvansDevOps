namespace AvansDevOps.Domain.States;

// STATE PATTERN: Concrete state for 'Todo' phase.
// From Todo, a backlog item can only move to Doing.
public class TodoState : IBacklogItemState
{
    public string Name => "Todo";

    public void MoveToTodo(BacklogItem item)
    {
        throw new InvalidOperationException("Backlog item is already in Todo.");
    }

    public void MoveToDoing(BacklogItem item)
    {
        item.SetState(new DoingState());
    }

    public void MoveToReadyForTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Todo to ReadyForTesting.");
    }

    public void MoveToTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Todo to Testing.");
    }

    public void MoveToTested(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Todo to Tested.");
    }

    public void MoveToDone(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from Todo to Done.");
    }
}
