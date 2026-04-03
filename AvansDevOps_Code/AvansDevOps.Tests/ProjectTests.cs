using AvansDevOps.Domain;
using AvansDevOps.Domain.Factory;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for Project creation and management (FR-01).
/// Verifies that projects can be created with a product owner,
/// and that backlog items and sprints can be added.
/// </summary>
public class ProjectTests
{
    [Fact]
    public void Project_Create_ShouldHaveCorrectNameAndOwner()
    {
        var owner = new Person("Product Owner", "po@avans.nl");
        var project = new Project("Avans DevOps", owner);

        Assert.Equal("Avans DevOps", project.Name);
        Assert.Equal("Product Owner", project.ProductOwner.Name);
    }

    [Fact]
    public void Project_AddBacklogItem_ShouldBeInProductBacklog()
    {
        var owner = new Person("PO", "po@avans.nl");
        var project = new Project("Test Project", owner);

        var item = project.AddBacklogItem("Login Feature", "Implement login", 5);

        Assert.Single(project.ProductBacklog);
        Assert.Equal("Login Feature", item.Title);
        Assert.Equal(5, item.StoryPoints);
    }

    [Fact]
    public void Project_CreateSprint_ShouldBeInSprintsList()
    {
        var owner = new Person("PO", "po@avans.nl");
        var project = new Project("Test Project", owner);

        var sprint = project.CreateSprint(
            new ReviewSprintFactory(),
            "Sprint 1",
            DateTime.Now,
            DateTime.Now.AddDays(14));

        Assert.Single(project.Sprints);
        Assert.Equal("Sprint 1", sprint.Name);
        Assert.IsType<ReviewSprint>(sprint);
    }

    [Fact]
    public void Project_FullSetup_BacklogItemsAndSprints()
    {
        var owner = new Person("PO", "po@avans.nl");
        var project = new Project("Avans DevOps", owner);

        project.AddBacklogItem("Feature A", "Description A", 3);
        project.AddBacklogItem("Feature B", "Description B", 5);

        var sprint = project.CreateSprint(
            new ReleaseSprintFactory(),
            "Release 1.0",
            DateTime.Now,
            DateTime.Now.AddDays(14));

        Assert.Equal(2, project.ProductBacklog.Count);
        Assert.Single(project.Sprints);
        Assert.IsType<ReleaseSprint>(sprint);
    }
}
