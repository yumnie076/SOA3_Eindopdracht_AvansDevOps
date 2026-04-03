using AvansDevOps.Domain;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.States;
using Moq;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for the Observer Pattern (notification system).
/// Uses Moq to mock ISubscriber and verify notifications are sent correctly.
/// </summary>
public class NotificationTests
{
    // === BacklogItem notifications ===

    [Fact]
    public void BacklogItem_MoveToReadyForTesting_ShouldNotifySubscribers()
    {
        var item = new BacklogItem("Feature X", "Description", 3);
        var mockSubscriber = new Mock<ISubscriber>();

        item.Subscribe(mockSubscriber.Object);
        item.MoveToDoing();
        item.MoveToReadyForTesting();

        mockSubscriber.Verify(
            s => s.Update(It.Is<string>(msg => msg.Contains("ready for testing"))),
            Times.Once);
    }

    [Fact]
    public void BacklogItem_RejectedFromReadyForTesting_ShouldNotifyScrumMaster()
    {
        var item = new BacklogItem("Feature X", "Description", 3);
        var mockSubscriber = new Mock<ISubscriber>();

        item.Subscribe(mockSubscriber.Object);
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTodo(); // Rejected by tester

        mockSubscriber.Verify(
            s => s.Update(It.Is<string>(msg => msg.Contains("rejected") || msg.Contains("back to Todo"))),
            Times.Once);
    }

    [Fact]
    public void BacklogItem_MoveToDone_ShouldNotifySubscribers()
    {
        var item = new BacklogItem("Feature X", "Description", 3);
        var mockSubscriber = new Mock<ISubscriber>();

        item.Subscribe(mockSubscriber.Object);
        item.MoveToDoing();
        item.MoveToReadyForTesting();
        item.MoveToTesting();
        item.MoveToTested();
        item.MoveToDone();

        mockSubscriber.Verify(
            s => s.Update(It.Is<string>(msg => msg.Contains("done"))),
            Times.Once);
    }

    [Fact]
    public void BacklogItem_UnsubscribedObserver_ShouldNotReceiveNotifications()
    {
        var item = new BacklogItem("Feature X", "Description", 3);
        var mockSubscriber = new Mock<ISubscriber>();

        item.Subscribe(mockSubscriber.Object);
        item.Unsubscribe(mockSubscriber.Object);

        item.MoveToDoing();
        item.MoveToReadyForTesting();

        mockSubscriber.Verify(s => s.Update(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void BacklogItem_MultipleSubscribers_AllShouldBeNotified()
    {
        var item = new BacklogItem("Feature X", "Description", 3);
        var mock1 = new Mock<ISubscriber>();
        var mock2 = new Mock<ISubscriber>();
        var mock3 = new Mock<ISubscriber>();

        item.Subscribe(mock1.Object);
        item.Subscribe(mock2.Object);
        item.Subscribe(mock3.Object);

        item.MoveToDoing();
        item.MoveToReadyForTesting();

        mock1.Verify(s => s.Update(It.IsAny<string>()), Times.Once);
        mock2.Verify(s => s.Update(It.IsAny<string>()), Times.Once);
        mock3.Verify(s => s.Update(It.IsAny<string>()), Times.Once);
    }

    // === Concrete subscriber tests ===

    [Fact]
    public void EmailSubscriber_ShouldReceiveFormattedMessage()
    {
        var person = new Person("Alice", "alice@avans.nl");
        var emailSub = new EmailSubscriber(person);

        emailSub.Update("Test notification");

        Assert.Single(emailSub.ReceivedMessages);
        Assert.Contains("EMAIL", emailSub.ReceivedMessages[0]);
        Assert.Contains("alice@avans.nl", emailSub.ReceivedMessages[0]);
    }

    [Fact]
    public void SlackSubscriber_ShouldReceiveFormattedMessage()
    {
        var person = new Person("Bob", "bob@avans.nl");
        var slackSub = new SlackSubscriber(person, "#dev-team");

        slackSub.Update("Test notification");

        Assert.Single(slackSub.ReceivedMessages);
        Assert.Contains("SLACK", slackSub.ReceivedMessages[0]);
        Assert.Contains("#dev-team", slackSub.ReceivedMessages[0]);
    }

    // === Sprint notifications ===

    [Fact]
    public void ReleaseSprint_StartRelease_ShouldNotifySubscribers()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        var mockSubscriber = new Mock<ISubscriber>();
        sprint.Subscribe(mockSubscriber.Object);

        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();

        mockSubscriber.Verify(
            s => s.Update(It.Is<string>(msg => msg.Contains("pipeline started"))),
            Times.Once);
    }

    [Fact]
    public void ReleaseSprint_CancelRelease_ShouldNotifyProductOwnerAndScrumMaster()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        var mockSubscriber = new Mock<ISubscriber>();
        sprint.Subscribe(mockSubscriber.Object);

        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();
        sprint.CancelRelease();

        mockSubscriber.Verify(
            s => s.Update(It.Is<string>(msg => msg.Contains("CANCELLED"))),
            Times.Once);
    }

    [Fact]
    public void ReleaseSprint_SuccessfulRelease_ShouldNotifySubscribers()
    {
        var sprint = new ReleaseSprint("Release 1.0", DateTime.Now, DateTime.Now.AddDays(14));
        var mockSubscriber = new Mock<ISubscriber>();
        sprint.Subscribe(mockSubscriber.Object);

        sprint.Start();
        sprint.Finish();
        sprint.StartRelease();
        sprint.FinishRelease();

        mockSubscriber.Verify(
            s => s.Update(It.Is<string>(msg => msg.Contains("SUCCESSFUL"))),
            Times.Once);
    }

    // === Combined email + Slack notifications ===

    [Fact]
    public void BacklogItem_MultipleMediaTypes_AllShouldReceive()
    {
        var item = new BacklogItem("Feature X", "Description", 3);
        var person = new Person("Alice", "alice@avans.nl");

        var emailSub = new EmailSubscriber(person);
        var slackSub = new SlackSubscriber(person, "#dev");

        item.Subscribe(emailSub);
        item.Subscribe(slackSub);

        item.MoveToDoing();
        item.MoveToReadyForTesting();

        Assert.Single(emailSub.ReceivedMessages);
        Assert.Single(slackSub.ReceivedMessages);
        Assert.Contains("EMAIL", emailSub.ReceivedMessages[0]);
        Assert.Contains("SLACK", slackSub.ReceivedMessages[0]);
    }
}
