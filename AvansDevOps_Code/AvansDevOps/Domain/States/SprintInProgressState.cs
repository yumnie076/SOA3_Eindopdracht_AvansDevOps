namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint is in progress (being executed).
// Backlog items cannot be added, sprint properties cannot be modified.
public class SprintInProgressState : ISprintState
{
    public string Name => "InProgress";

    public void Start(Sprint sprint) =>
        throw new InvalidOperationException("Sprint is already in progress.");

    public void Finish(Sprint sprint)
    {
        sprint.SetState(new SprintFinishedState());
    }

    public void StartReview(Sprint sprint) =>
        throw new InvalidOperationException("Sprint must be finished before review.");

    public void StartRelease(Sprint sprint) =>
        throw new InvalidOperationException("Sprint must be finished before release.");

    public void CancelRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release in progress.");

    public void FinishRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release in progress.");

    public void Close(Sprint sprint) =>
        throw new InvalidOperationException("Cannot close a sprint that is in progress.");

    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
