namespace AvansDevOps.Domain;

using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.States;

// Uses STATE PATTERN for lifecycle management and
// OBSERVER PATTERN for notification of state changes.
public class BacklogItem : INotificationPublisher
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int StoryPoints { get; set; }
    public Person? AssignedDeveloper { get; private set; }
    public IBacklogItemState State { get; private set; }
    public List<Activity> Activities { get; } = new();
    public bool IsClosed { get; set; }

    private readonly List<ISubscriber> _subscribers = new();

    public BacklogItem(string title, string description, int storyPoints = 0)
    {
        Title = title;
        Description = description;
        StoryPoints = storyPoints;
        State = new TodoState();
        IsClosed = false;
    }

    // STATE PATTERN: Delegate state transitions to the current state object
    public void MoveToTodo() => State.MoveToTodo(this);
    public void MoveToDoing() => State.MoveToDoing(this);
    public void MoveToReadyForTesting() => State.MoveToReadyForTesting(this);
    public void MoveToTesting() => State.MoveToTesting(this);
    public void MoveToTested() => State.MoveToTested(this);
    public void MoveToDone()
    {
        // Business rule: a backlog item can only be done if all activities are done
        if (Activities.Any() && Activities.Any(a => !a.IsDone))
        {
            throw new InvalidOperationException(
                "Cannot mark backlog item as Done: not all activities are completed.");
        }
        State.MoveToDone(this);
    }

    public void SetState(IBacklogItemState newState)
    {
        State = newState;
    }

    public void AssignDeveloper(Person developer)
    {
        AssignedDeveloper = developer;
    }

    public void AddActivity(Activity activity)
    {
        Activities.Add(activity);
    }

    // OBSERVER PATTERN: Manage subscribers
    public void Subscribe(ISubscriber subscriber)
    {
        if (!_subscribers.Contains(subscriber))
            _subscribers.Add(subscriber);
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public void NotifySubscribers(string message)
    {
        foreach (var subscriber in _subscribers.ToList())
        {
            subscriber.Update(message);
        }
    }

    public string GetStateName() => State.Name;
}
