namespace AvansDevOps.Domain.Notifications;

// OBSERVER PATTERN: Concrete subscriber for Slack notifications.
// Stub implementation for Slack integration.
public class SlackSubscriber : ISubscriber
{
    public Person Person { get; }
    public string SlackChannel { get; }
    public List<string> ReceivedMessages { get; } = new();

    public SlackSubscriber(Person person, string slackChannel = "#general")
    {
        Person = person;
        SlackChannel = slackChannel;
    }

    public void Update(string message)
    {
        // Stub: in production, this would use the Slack API
        string formattedMessage = $"[SLACK {SlackChannel} @{Person.Name}]: {message}";
        ReceivedMessages.Add(formattedMessage);
        Console.WriteLine(formattedMessage);
    }
}
