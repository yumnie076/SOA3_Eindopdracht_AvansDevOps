namespace AvansDevOps.Domain.States;

// STATE PATTERN: Defines the interface for all backlog item states.
// Each concrete state encapsulates the behavior for a specific phase
// of a backlog item's lifecycle, following the Open/Closed Principle.
public interface IBacklogItemState
{
    string Name { get; }
    void MoveToTodo(BacklogItem item);
    void MoveToDoing(BacklogItem item);
    void MoveToReadyForTesting(BacklogItem item);
    void MoveToTesting(BacklogItem item);
    void MoveToTested(BacklogItem item);
    void MoveToDone(BacklogItem item);
}
