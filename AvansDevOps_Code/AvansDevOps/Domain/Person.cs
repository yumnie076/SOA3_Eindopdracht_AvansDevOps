namespace AvansDevOps.Domain;

public class Person
{
    public string Name { get; set; }
    public string Email { get; set; }

    public Person(string name, string email)
    {
        Name = name;
        Email = email;
    }
}

public enum Role
{
    Developer,
    ScrumMaster,
    ProductOwner,
    Tester,
    LeadDeveloper
}
