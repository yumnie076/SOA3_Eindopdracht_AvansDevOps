namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint has been successfully released. This is an end state.
public class SprintReleasedState : ISprintState
{
    public string Name => "Released";

    public void Start(Sprint sprint) => throw new InvalidOperationException("Sprint is released and closed.");
    public void Finish(Sprint sprint) => throw new InvalidOperationException("Sprint is released and closed.");
    public void StartReview(Sprint sprint) => throw new InvalidOperationException("Sprint is released and closed.");
    public void StartRelease(Sprint sprint) => throw new InvalidOperationException("Sprint is released and closed.");
    public void CancelRelease(Sprint sprint) => throw new InvalidOperationException("Sprint is released and closed.");
    public void FinishRelease(Sprint sprint) => throw new InvalidOperationException("Sprint is released and closed.");
    public void Close(Sprint sprint) => throw new InvalidOperationException("Sprint is already closed.");
    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
