using AvansDevOps.Domain;
using AvansDevOps.Domain.SCM;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for SCM integration (FR-19).
/// Verifies that repositories, branches, and commits can be created
/// and linked to projects.
/// </summary>
public class RepositoryTests
{
    [Fact]
    public void Repository_Create_ShouldHaveMainBranch()
    {
        var repo = new Repository("avans-devops");

        Assert.Equal("avans-devops", repo.Name);
        Assert.Single(repo.Branches);
        Assert.Equal("main", repo.MainBranch.Name);
    }

    [Fact]
    public void Repository_CreateBranch_ShouldBeAdded()
    {
        var repo = new Repository("avans-devops");

        var feature = repo.CreateBranch("feature/login");

        Assert.Equal(2, repo.Branches.Count);
        Assert.Equal("feature/login", feature.Name);
    }

    [Fact]
    public void Repository_CreateDuplicateBranch_ShouldThrow()
    {
        var repo = new Repository("avans-devops");
        repo.CreateBranch("develop");

        Assert.Throws<InvalidOperationException>(() => repo.CreateBranch("develop"));
    }

    [Fact]
    public void Branch_AddCommit_ShouldBeInCommitsList()
    {
        var repo = new Repository("avans-devops");
        var dev = new Person("Alice", "alice@avans.nl");

        var commit = repo.MainBranch.AddCommit("Initial commit", dev);

        Assert.Single(repo.MainBranch.Commits);
        Assert.Equal("Initial commit", commit.Message);
        Assert.Equal("Alice", commit.Author.Name);
    }

    [Fact]
    public void Repository_LinkToProject_ShouldWork()
    {
        var owner = new Person("PO", "po@avans.nl");
        var project = new Project("Avans DevOps", owner);
        var repo = new Repository("avans-devops");

        project.SetRepository(repo);

        Assert.NotNull(project.Repository);
        Assert.Equal("avans-devops", project.Repository!.Name);
    }

    [Fact]
    public void FullSCMFlow_CreateBranchCommitAndLink()
    {
        var owner = new Person("PO", "po@avans.nl");
        var project = new Project("Avans DevOps", owner);
        var repo = new Repository("avans-devops");
        project.SetRepository(repo);

        var dev = new Person("Bob", "bob@avans.nl");
        var featureBranch = repo.CreateBranch("feature/backlog-states");
        featureBranch.AddCommit("Add TodoState", dev);
        featureBranch.AddCommit("Add DoingState", dev);

        Assert.Equal(2, repo.Branches.Count); // main + feature
        Assert.Equal(2, featureBranch.Commits.Count);
        Assert.NotNull(repo.GetBranch("feature/backlog-states"));
    }
}
