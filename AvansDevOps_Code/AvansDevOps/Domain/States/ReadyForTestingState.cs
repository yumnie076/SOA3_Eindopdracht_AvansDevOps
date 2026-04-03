namespace AvansDevOps.Domain.States;

// STATE PATTERN: Concrete state for 'ReadyForTesting' phase.
// From ReadyForTesting, item can move to Testing or back to Todo (if rejected).
public class ReadyForTestingState : IBacklogItemState
{
    public string Name => "ReadyForTesting";

    public void MoveToTodo(BacklogItem item)
    {
        // Tester found issues - item goes back to Todo
        item.SetState(new TodoState());
        // OBSERVER PATTERN: Notify scrum master about the rejection
        item.NotifySubscribers($"Backlog item '{item.Title}' was rejected by tester and moved back to Todo.");
    }

    public void MoveToDoing(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move from ReadyForTesting to Doing. Must go back to Todo first.");
    }

    public void MoveToReadyForTesting(BacklogItem item)
    {
        throw new InvalidOperationException("Backlog item is already in ReadyForTesting.");
    }

    public void MoveToTesting(BacklogItem item)
    {
        item.SetState(new TestingState());
    }

    public void MoveToTested(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from ReadyForTesting to Tested.");
    }

    public void MoveToDone(BacklogItem item)
    {
        throw new InvalidOperationException("Cannot move directly from ReadyForTesting to Done.");
    }
}
