using AvansDevOps.Domain;
using AvansDevOps.Domain.Factory;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for the Factory Method Pattern (Sprint creation).
/// Verifies that the correct sprint types are created by each factory.
/// </summary>
public class SprintFactoryTests
{
    [Fact]
    public void ReviewSprintFactory_ShouldCreateReviewSprint()
    {
        ISprintFactory factory = new ReviewSprintFactory();
        var sprint = factory.CreateSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));

        Assert.IsType<ReviewSprint>(sprint);
        Assert.Equal("Sprint 1", sprint.Name);
    }

    [Fact]
    public void ReleaseSprintFactory_ShouldCreateReleaseSprint()
    {
        ISprintFactory factory = new ReleaseSprintFactory();
        var sprint = factory.CreateSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));

        Assert.IsType<ReleaseSprint>(sprint);
        Assert.Equal("Release 1.0", sprint.Name);
    }

    [Fact]
    public void Project_CreateSprint_UsingFactory_ShouldWork()
    {
        var owner = new Person("Product Owner", "po@avans.nl");
        var project = new Project("SOA3 Project", owner);

        var sprint = project.CreateSprint(
            new ReviewSprintFactory(),
            "Sprint 1",
            DateTime.Now,
            DateTime.Now.AddDays(14));

        Assert.Single(project.Sprints);
        Assert.IsType<ReviewSprint>(sprint);
    }

    [Fact]
    public void Project_CreateMultipleSprints_DifferentFactories_ShouldWork()
    {
        var owner = new Person("Product Owner", "po@avans.nl");
        var project = new Project("SOA3 Project", owner);

        project.CreateSprint(new ReviewSprintFactory(), "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        project.CreateSprint(new ReviewSprintFactory(), "Sprint 2", DateTime.Now.AddDays(14), DateTime.Now.AddDays(28));
        project.CreateSprint(new ReleaseSprintFactory(), "Release 1.0", DateTime.Now.AddDays(28), DateTime.Now.AddDays(42));

        Assert.Equal(3, project.Sprints.Count);
        Assert.IsType<ReviewSprint>(project.Sprints[0]);
        Assert.IsType<ReviewSprint>(project.Sprints[1]);
        Assert.IsType<ReleaseSprint>(project.Sprints[2]);
    }

    [Fact]
    public void Factory_CreatedSprint_ShouldBeInCreatedState()
    {
        ISprintFactory factory = new ReviewSprintFactory();
        var sprint = factory.CreateSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));

        Assert.Equal("Created", sprint.GetStateName());
    }

    [Fact]
    public void Factory_CreatedSprint_ShouldHaveCorrectDates()
    {
        var start = new DateTime(2025, 3, 1);
        var end = new DateTime(2025, 3, 14);

        ISprintFactory factory = new ReleaseSprintFactory();
        var sprint = factory.CreateSprint("Release", start, end);

        Assert.Equal(start, sprint.StartDate);
        Assert.Equal(end, sprint.EndDate);
    }
}
