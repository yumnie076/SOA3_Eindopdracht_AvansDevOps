namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint has finished (time is up).
// Depending on sprint type, can go to Review or Release.
public class SprintFinishedState : ISprintState
{
    public string Name => "Finished";

    public void Start(Sprint sprint) =>
        throw new InvalidOperationException("Cannot start a finished sprint.");

    public void Finish(Sprint sprint) =>
        throw new InvalidOperationException("Sprint is already finished.");

    public void StartReview(Sprint sprint)
    {
        if (sprint is not ReviewSprint)
            throw new InvalidOperationException("Only review sprints can start a review.");
        sprint.SetState(new SprintReviewState());
    }

    public void StartRelease(Sprint sprint)
    {
        if (sprint is not ReleaseSprint)
            throw new InvalidOperationException("Only release sprints can start a release.");
        sprint.SetState(new SprintReleasingState());
        // OBSERVER PATTERN: Notify that release pipeline is starting
        sprint.NotifySubscribers($"Sprint '{sprint.Name}': release pipeline started.");
    }

    public void CancelRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release in progress.");

    public void FinishRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release in progress.");

    public void Close(Sprint sprint) =>
        throw new InvalidOperationException("Sprint must go through review or release before closing.");

    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
