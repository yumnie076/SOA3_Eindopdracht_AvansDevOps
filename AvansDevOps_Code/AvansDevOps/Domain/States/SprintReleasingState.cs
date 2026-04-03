namespace AvansDevOps.Domain.States;

// STATE PATTERN: Release pipeline is executing.
// Sprint cannot be modified during pipeline execution.
public class SprintReleasingState : ISprintState
{
    public string Name => "Releasing";

    public void Start(Sprint sprint) =>
        throw new InvalidOperationException("Cannot start a sprint during release.");

    public void Finish(Sprint sprint) =>
        throw new InvalidOperationException("Sprint is already finished.");

    public void StartReview(Sprint sprint) =>
        throw new InvalidOperationException("Cannot start review during release.");

    public void StartRelease(Sprint sprint) =>
        throw new InvalidOperationException("Release already in progress.");

    public void CancelRelease(Sprint sprint)
    {
        sprint.SetState(new SprintCancelledState());
        sprint.NotifySubscribers(
            $"Sprint '{sprint.Name}': release CANCELLED. Product owner and scrum master notified.");
    }

    public void FinishRelease(Sprint sprint)
    {
        sprint.SetState(new SprintReleasedState());
        sprint.NotifySubscribers(
            $"Sprint '{sprint.Name}': release SUCCESSFUL. Sprint is now closed.");
    }

    public void Close(Sprint sprint) =>
        throw new InvalidOperationException("Cannot close sprint while release is in progress.");

    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
