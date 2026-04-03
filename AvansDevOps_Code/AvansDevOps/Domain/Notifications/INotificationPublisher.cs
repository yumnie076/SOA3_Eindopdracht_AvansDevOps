namespace AvansDevOps.Domain.Notifications;

// OBSERVER PATTERN: Publisher interface (Subject).
// Objects that generate notifications implement this interface.
// Subscribers can be added/removed dynamically at runtime,
// supporting the Open/Closed Principle.
public interface INotificationPublisher
{
    void Subscribe(ISubscriber subscriber);
    void Unsubscribe(ISubscriber subscriber);
    void NotifySubscribers(string message);
}
