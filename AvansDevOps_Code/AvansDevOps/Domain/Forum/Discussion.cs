namespace AvansDevOps.Domain.Forum;

public class Discussion
{
    public string Title { get; set; }
    public Person Author { get; }
    public Forum Forum { get; }
    public List<Message> Messages { get; } = new();
    public DateTime CreatedAt { get; }

    public Discussion(string title, Person author, Forum forum)
    {
        Title = title;
        Author = author;
        Forum = forum;
        CreatedAt = DateTime.Now;
    }

    public Message AddMessage(string body, Person author)
    {
        if (Forum.IsLocked)
            throw new InvalidOperationException(
                "Cannot add messages: the backlog item is closed and the forum is locked.");

        var message = new Message(body, author);
        Messages.Add(message);

        // OBSERVER PATTERN: Notify team members about new message
        Forum.NotifySubscribers(
            $"New message in '{Title}' by {author.Name}: {(body.Length > 50 ? body[..50] + "..." : body)}");

        return message;
    }
}
