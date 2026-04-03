namespace AvansDevOps.Domain.States;

// STATE PATTERN: Interface for sprint lifecycle states.
// Sprints go through: Created -> InProgress -> Finished -> (Review/Releasing) -> Closed/Cancelled
public interface ISprintState
{
    string Name { get; }
    void Start(Sprint sprint);
    void Finish(Sprint sprint);
    void StartReview(Sprint sprint);
    void StartRelease(Sprint sprint);
    void CancelRelease(Sprint sprint);
    void FinishRelease(Sprint sprint);
    void Close(Sprint sprint);
    bool CanModifySprintProperties();
    bool CanAddBacklogItems();
}
