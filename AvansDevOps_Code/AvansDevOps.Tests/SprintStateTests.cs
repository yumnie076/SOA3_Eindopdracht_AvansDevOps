using AvansDevOps.Domain;
using AvansDevOps.Domain.Pipeline;
using AvansDevOps.Domain.States;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for Sprint state transitions (State Pattern).
/// Covers both ReviewSprint and ReleaseSprint lifecycle flows.
/// </summary>
public class SprintStateTests
{
    // === ReviewSprint lifecycle ===

    [Fact]
    public void NewReviewSprint_ShouldBeInCreatedState()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        Assert.Equal("Created", sprint.GetStateName());
    }

    [Fact]
    public void ReviewSprint_FullLifecycle_ShouldSucceed()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));

        sprint.Start();
        Assert.Equal("InProgress", sprint.GetStateName());

        sprint.Finish();
        Assert.Equal("Finished", sprint.GetStateName());

        sprint.StartReview();
        Assert.Equal("Review", sprint.GetStateName());

        sprint.UploadReviewSummary("Sprint review went well. All items completed.");
        sprint.Close();
        Assert.Equal("Closed", sprint.GetStateName());
    }

    [Fact]
    public void ReviewSprint_CloseWithoutSummary_ShouldThrow()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();
        sprint.StartReview();

        Assert.Throws<InvalidOperationException>(() => sprint.Close());
    }

    [Fact]
    public void ReviewSprint_CannotStartRelease_ShouldThrow()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();

        Assert.Throws<InvalidOperationException>(() => sprint.StartRelease());
    }

    // === ReleaseSprint lifecycle ===

    [Fact]
    public void ReleaseSprint_FullLifecycle_SuccessfulRelease()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));

        sprint.Start();
        Assert.Equal("InProgress", sprint.GetStateName());

        sprint.Finish();
        Assert.Equal("Finished", sprint.GetStateName());

        sprint.StartRelease();
        Assert.Equal("Releasing", sprint.GetStateName());

        sprint.FinishRelease();
        Assert.Equal("Released", sprint.GetStateName());
    }

    [Fact]
    public void ReleaseSprint_CancelledRelease_ShouldBeCancelled()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();

        sprint.CancelRelease();
        Assert.Equal("Cancelled", sprint.GetStateName());
    }

    [Fact]
    public void ReleaseSprint_RetryAfterCancel_ShouldSucceed()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();
        sprint.CancelRelease();

        // Scrum master retries
        sprint.StartRelease();
        Assert.Equal("Releasing", sprint.GetStateName());

        sprint.FinishRelease();
        Assert.Equal("Released", sprint.GetStateName());
    }

    [Fact]
    public void ReleaseSprint_CannotStartReview_ShouldThrow()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();

        Assert.Throws<InvalidOperationException>(() => sprint.StartReview());
    }

    // === Property modification rules ===

    [Fact]
    public void CreatedSprint_CanModifyProperties()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.SetName("Updated Sprint 1");
        Assert.Equal("Updated Sprint 1", sprint.Name);
    }

    [Fact]
    public void InProgressSprint_CannotModifyProperties()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        Assert.Throws<InvalidOperationException>(() => sprint.SetName("Updated"));
    }

    [Fact]
    public void CreatedSprint_CanAddBacklogItems()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        var item = new BacklogItem("Item 1", "Description", 3);
        sprint.AddBacklogItem(item);
        Assert.Single(sprint.BacklogItems);
    }

    [Fact]
    public void InProgressSprint_CannotAddBacklogItems()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        var item = new BacklogItem("Item 1", "Description", 3);
        Assert.Throws<InvalidOperationException>(() => sprint.AddBacklogItem(item));
    }

    // === Scrum Master rule ===

    [Fact]
    public void Sprint_CanOnlyHaveOneScrumMaster()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        var sm1 = new Person("Alice", "alice@avans.nl");
        var sm2 = new Person("Bob", "bob@avans.nl");

        sprint.AddMember(sm1, Role.ScrumMaster);
        Assert.Throws<InvalidOperationException>(() => sprint.AddMember(sm2, Role.ScrumMaster));
    }

    // === End states are truly final ===

    [Fact]
    public void ReleasedSprint_CannotBeModified()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();
        sprint.FinishRelease();

        Assert.Throws<InvalidOperationException>(() => sprint.Start());
        Assert.Throws<InvalidOperationException>(() => sprint.Finish());
        Assert.Throws<InvalidOperationException>(() => sprint.Close());
    }

    [Fact]
    public void ClosedSprint_CannotBeModified()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();
        sprint.StartReview();
        sprint.UploadReviewSummary("Summary");
        sprint.Close();

        Assert.Throws<InvalidOperationException>(() => sprint.Start());
        Assert.Throws<InvalidOperationException>(() => sprint.StartReview());
    }

    // === Cannot skip states ===

    [Fact]
    public void CreatedSprint_CannotFinish()
    {
        var sprint = new ReviewSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
        Assert.Throws<InvalidOperationException>(() => sprint.Finish());
    }

    [Fact]
    public void InProgressSprint_CannotStartRelease()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        Assert.Throws<InvalidOperationException>(() => sprint.StartRelease());
    }

    [Fact]
    public void ReleasingSprint_CannotClose()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();
        Assert.Throws<InvalidOperationException>(() => sprint.Close());
    }
}
