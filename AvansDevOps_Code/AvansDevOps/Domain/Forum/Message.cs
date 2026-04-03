namespace AvansDevOps.Domain.Forum;

public class Message
{
    public string Body { get; set; }
    public Person Author { get; }
    public DateTime CreatedAt { get; }

    public Message(string body, Person author)
    {
        Body = body;
        Author = author;
        CreatedAt = DateTime.Now;
    }
}
