namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint has been closed after review. This is an end state.
public class SprintClosedState : ISprintState
{
    public string Name => "Closed";

    public void Start(Sprint sprint) => throw new InvalidOperationException("Sprint is closed.");
    public void Finish(Sprint sprint) => throw new InvalidOperationException("Sprint is closed.");
    public void StartReview(Sprint sprint) => throw new InvalidOperationException("Sprint is closed.");
    public void StartRelease(Sprint sprint) => throw new InvalidOperationException("Sprint is closed.");
    public void CancelRelease(Sprint sprint) => throw new InvalidOperationException("Sprint is closed.");
    public void FinishRelease(Sprint sprint) => throw new InvalidOperationException("Sprint is closed.");
    public void Close(Sprint sprint) => throw new InvalidOperationException("Sprint is already closed.");
    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
