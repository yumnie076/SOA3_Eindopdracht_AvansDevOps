namespace AvansDevOps.Domain;

using AvansDevOps.Domain.States;

public class Activity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Person? AssignedDeveloper { get; set; }
    public bool IsDone { get; set; }

    public Activity(string title, string description)
    {
        Title = title;
        Description = description;
        IsDone = false;
    }

    public void MarkDone()
    {
        IsDone = true;
    }
}
