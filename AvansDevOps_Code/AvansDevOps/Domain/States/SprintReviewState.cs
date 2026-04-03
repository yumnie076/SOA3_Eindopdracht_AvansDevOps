namespace AvansDevOps.Domain.States;

// STATE PATTERN: Sprint review is taking place.
// Scrum master must upload a review summary document before closing.
public class SprintReviewState : ISprintState
{
    public string Name => "Review";

    public void Start(Sprint sprint) =>
        throw new InvalidOperationException("Cannot start a sprint during review.");

    public void Finish(Sprint sprint) =>
        throw new InvalidOperationException("Sprint is already finished.");

    public void StartReview(Sprint sprint) =>
        throw new InvalidOperationException("Review already in progress.");

    public void StartRelease(Sprint sprint) =>
        throw new InvalidOperationException("Cannot release during review.");

    public void CancelRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release to cancel.");

    public void FinishRelease(Sprint sprint) =>
        throw new InvalidOperationException("No release in progress.");

    public void Close(Sprint sprint)
    {
        if (sprint is ReviewSprint reviewSprint && !reviewSprint.HasReviewSummary)
        {
            throw new InvalidOperationException(
                "Cannot close review sprint without uploading the review summary document.");
        }
        sprint.SetState(new SprintClosedState());
        sprint.NotifySubscribers($"Sprint '{sprint.Name}': review completed and sprint closed.");
    }

    public bool CanModifySprintProperties() => false;
    public bool CanAddBacklogItems() => false;
}
