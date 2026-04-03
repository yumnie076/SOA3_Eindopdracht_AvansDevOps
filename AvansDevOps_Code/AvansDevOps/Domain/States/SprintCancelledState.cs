namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint release was cancelled. This is an end state.
// Automatic notification is sent to product owner and scrum master.
public class SprintCancelledState : ISprintState
{
    public string Name => "Cancelled";

    public void Start(Sprint sprint) => throw new InvalidOperationException("Sprint is cancelled.");
    public void Finish(Sprint sprint) => throw new InvalidOperationException("Sprint is cancelled.");
    public void StartReview(Sprint sprint) => throw new InvalidOperationException("Sprint is cancelled.");

    public void StartRelease(Sprint sprint)
    {
        // Scrum master can retry the release after cancellation
        sprint.SetState(new SprintReleasingState());
        sprint.NotifySubscribers($"Sprint '{sprint.Name}': retrying release pipeline.");
    }

    public void CancelRelease(Sprint sprint) => throw new InvalidOperationException("Release already cancelled.");
    public void FinishRelease(Sprint sprint) => throw new InvalidOperationException("No release in progress.");
    public void Close(Sprint sprint) => throw new InvalidOperationException("Sprint is cancelled, cannot close normally.");
    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
