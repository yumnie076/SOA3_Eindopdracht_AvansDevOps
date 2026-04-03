namespace AvansDevOps.Domain;

public class SprintMember
{
    public Person Person { get; }
    public Role Role { get; }

    public SprintMember(Person person, Role role)
    {
        Person = person;
        Role = role;
    }
}
