namespace AvansDevOps.Domain.SCM;

// SCM integration - simplified representation of version control concepts.
// In production, this would integrate with Git, Subversion, etc.

public class Repository
{
    public string Name { get; }
    public List<Branch> Branches { get; } = new();
    public Branch MainBranch { get; }

    public Repository(string name)
    {
        Name = name;
        MainBranch = new Branch("main");
        Branches.Add(MainBranch);
    }

    public Branch CreateBranch(string name)
    {
        if (Branches.Any(b => b.Name == name))
            throw new InvalidOperationException($"Branch '{name}' already exists.");

        var branch = new Branch(name);
        Branches.Add(branch);
        return branch;
    }

    public Branch? GetBranch(string name) =>
        Branches.FirstOrDefault(b => b.Name == name);
}

public class Branch
{
    public string Name { get; }
    public List<Commit> Commits { get; } = new();

    public Branch(string name)
    {
        Name = name;
    }

    public Commit AddCommit(string message, Person author)
    {
        var commit = new Commit(message, author);
        Commits.Add(commit);
        return commit;
    }
}

public class Commit
{
    public string Id { get; }
    public string Message { get; }
    public Person Author { get; }
    public DateTime Timestamp { get; }

    public Commit(string message, Person author)
    {
        Id = Guid.NewGuid().ToString("N")[..8];
        Message = message;
        Author = author;
        Timestamp = DateTime.Now;
    }
}
