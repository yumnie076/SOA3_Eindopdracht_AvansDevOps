namespace AvansDevOps.Domain.Forum;

using AvansDevOps.Domain.Notifications;

// Forum is linked to a BacklogItem.
// When the backlog item is closed (done + sprint finished), the forum is locked.
public class Forum : INotificationPublisher
{
    public BacklogItem BacklogItem { get; }
    public List<Discussion> Discussions { get; } = new();

    private readonly List<ISubscriber> _subscribers = new();

    public Forum(BacklogItem backlogItem)
    {
        BacklogItem = backlogItem;
    }

    public bool IsLocked => BacklogItem.IsClosed;

    public Discussion StartDiscussion(string title, Person author)
    {
        if (IsLocked)
            throw new InvalidOperationException(
                "Cannot start new discussions: the backlog item is closed.");

        var discussion = new Discussion(title, author, this);
        Discussions.Add(discussion);
        NotifySubscribers($"New discussion started: '{title}' by {author.Name}");
        return discussion;
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
}
