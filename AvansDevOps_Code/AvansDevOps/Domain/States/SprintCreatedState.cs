namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint has been created but not yet started.
// Properties can be modified and backlog items can be added.
public class SprintCreatedState : ISprintState
{
    public string Name => "Created";

    public void Start(Sprint sprint)
    {
        sprint.SetState(new SprintInProgressState());
    }

    public void Finish(Sprint sprint) =>
        throw new InvalidOperationException("Cannot finish a sprint that hasn't started.");

    public void StartReview(Sprint sprint) =>
        throw new InvalidOperationException("Cannot start review on a sprint that hasn't started.");

    public void StartRelease(Sprint sprint) =>
        throw new InvalidOperationException("Cannot release a sprint that hasn't started.");

    public void CancelRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release to cancel.");

    public void FinishRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release to finish.");

    public void Close(Sprint sprint) =>
        throw new InvalidOperationException("Cannot close a sprint that hasn't started.");

    public bool CanModifySprintProperties() => true;
    public bool CanAddBacklogItems() => true;
}
