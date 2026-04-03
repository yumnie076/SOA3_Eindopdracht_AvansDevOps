namespace AvansDevOps.Domain.Notifications;

// OBSERVER PATTERN: Concrete subscriber for email notifications.
// This is a stub implementation; in production, this would integrate
// with an actual email service (SMTP, SendGrid, etc.).
public class EmailSubscriber : ISubscriber
{
    public Person Person { get; }
    public List<string> ReceivedMessages { get; } = new();

    public EmailSubscriber(Person person)
    {
        Person = person;
    }

    public void Update(string message)
    {
        // Stub: in production, this would send an actual email
        string formattedMessage = $"[EMAIL to {Person.Email}]: {message}";
        ReceivedMessages.Add(formattedMessage);
        Console.WriteLine(formattedMessage);
    }
}
