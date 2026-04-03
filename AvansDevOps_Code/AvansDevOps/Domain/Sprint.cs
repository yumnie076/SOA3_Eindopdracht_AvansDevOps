namespace AvansDevOps.Domain;

using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.Pipeline;
using AvansDevOps.Domain.States;

// Uses STATE PATTERN for lifecycle management and
// OBSERVER PATTERN for notification of sprint events.
public abstract class Sprint : INotificationPublisher
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ISprintState State { get; private set; }
    public List<BacklogItem> BacklogItems { get; } = new();
    public List<SprintMember> Members { get; } = new();
    public DevelopmentPipeline? Pipeline { get; set; }

    private readonly List<ISubscriber> _subscribers = new();

    protected Sprint(string name, DateTime startDate, DateTime endDate)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        State = new SprintCreatedState();
    }

    public void SetState(ISprintState newState)
    {
        State = newState;
    }

    // STATE PATTERN: Delegate actions to current state
    public void Start() => State.Start(this);
    public void Finish() => State.Finish(this);
    public void StartRelease() => State.StartRelease(this);
    public void CancelRelease() => State.CancelRelease(this);
    public void FinishRelease() => State.FinishRelease(this);
    public void Close() => State.Close(this);
    public void StartReview() => State.StartReview(this);

    public void SetName(string name)
    {
        if (!State.CanModifySprintProperties())
            throw new InvalidOperationException($"Cannot modify sprint properties in state '{State.Name}'.");
        Name = name;
    }

    public void SetDates(DateTime start, DateTime end)
    {
        if (!State.CanModifySprintProperties())
            throw new InvalidOperationException($"Cannot modify sprint properties in state '{State.Name}'.");
        StartDate = start;
        EndDate = end;
    }

    public void AddBacklogItem(BacklogItem item)
    {
        if (!State.CanAddBacklogItems())
            throw new InvalidOperationException($"Cannot add backlog items in state '{State.Name}'.");
        BacklogItems.Add(item);
    }

    public void AddMember(Person person, Role role)
    {
        if (role == Role.ScrumMaster && Members.Any(m => m.Role == Role.ScrumMaster))
            throw new InvalidOperationException("A sprint can only have one Scrum Master.");
        Members.Add(new SprintMember(person, role));
    }

    public Person? GetScrumMaster() =>
        Members.FirstOrDefault(m => m.Role == Role.ScrumMaster)?.Person;

    // Execute the associated development pipeline
    public bool ExecutePipeline()
    {
        if (Pipeline == null)
            throw new InvalidOperationException("No pipeline configured for this sprint.");
        return Pipeline.Execute();
    }

    // OBSERVER PATTERN
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
