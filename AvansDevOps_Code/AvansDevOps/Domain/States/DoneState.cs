namespace AvansDevOps.Domain.States;

// STATE PATTERN: Concrete state for 'Done' phase.
// This is the final state. No transitions are allowed except back to Todo
// (e.g., when a tester finds a regression after the item was marked done).
public class DoneState : IBacklogItemState
{
    public string Name => "Done";

    public void MoveToTodo(BacklogItem item)
    {
        // Regression found - item reopened
        item.SetState(new TodoState());
        item.NotifySubscribers($"Backlog item '{item.Title}' was reopened and moved back to Todo.");
    }

    public void MoveToDoing(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Done to Doing. Must go via Todo.");
    }

    public void MoveToReadyForTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Done to ReadyForTesting.");
    }

    public void MoveToTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Done to Testing.");
    }

    public void MoveToTested(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Done to Tested.");
    }

    public void MoveToDone(BacklogItem item)
    {
        throw new InvalidOperationException("Backlog item is already Done.");
    }
}
