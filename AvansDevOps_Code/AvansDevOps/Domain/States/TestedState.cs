namespace AvansDevOps.Domain.States;

// STATE PATTERN: Concrete state for 'Tested' phase.
// A lead developer checks the Definition of Done.
// From Tested, item can move to Done or back to ReadyForTesting.
public class TestedState : IBacklogItemState
{
    public string Name => "Tested";

    public void MoveToTodo(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Tested directly to Todo.");
    }

    public void MoveToDoing(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Tested to Doing.");
    }

    public void MoveToReadyForTesting(BacklogItem item)
    {
        // Definition of Done not met - goes back to ReadyForTesting
        item.SetState(new ReadyForTestingState());
        item.NotifySubscribers($"Backlog item '{item.Title}' did not meet Definition of Done. Sent back for re-testing.");
    }

    public void MoveToTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from Tested to Testing.");
    }

    public void MoveToTested(BacklogItem item)
    {
        throw new InvalidOperationException("Backlog item is already in Tested.");
    }

    public void MoveToDone(BacklogItem item)
    {
        item.SetState(new DoneState());
        item.NotifySubscribers($"Backlog item '{item.Title}' is done!");
    }
}
