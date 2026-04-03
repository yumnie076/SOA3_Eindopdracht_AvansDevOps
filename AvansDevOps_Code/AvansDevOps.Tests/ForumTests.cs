using AvansDevOps.Domain;
using AvansDevOps.Domain.Forum;
using AvansDevOps.Domain.Notifications;
using Moq;
using Xunit;

namespace AvansDevOps.Tests;

/// <summary>
/// Tests for Forum, Discussion, and Message functionality.
/// Verifies locking behavior when backlog items are closed.
/// </summary>
public class ForumTests
{
    private Person CreatePerson(string name = "Alice") =>
        new(name, $"{name.ToLower()}@avans.nl");

    [Fact]
    public void Forum_CanStartDiscussion_WhenBacklogItemIsOpen()
    {
        var item = new BacklogItem("Feature X", "Description");
        var forum = new Forum(item);
        var person = CreatePerson();

        var discussion = forum.StartDiscussion("How to implement?", person);

        Assert.Single(forum.Discussions);
        Assert.Equal("How to implement?", discussion.Title);
    }

    [Fact]
    public void Forum_CannotStartDiscussion_WhenBacklogItemIsClosed()
    {
        var item = new BacklogItem("Feature X", "Description");
        item.IsClosed = true;
        var forum = new Forum(item);

        Assert.Throws<InvalidOperationException>(
            () => forum.StartDiscussion("New topic", CreatePerson()));
    }

    [Fact]
    public void Discussion_CanAddMessage_WhenOpen()
    {
        var item = new BacklogItem("Feature X", "Description");
        var forum = new Forum(item);
        var person = CreatePerson();

        var discussion = forum.StartDiscussion("Topic", person);
        var message = discussion.AddMessage("I think we should use strategy pattern", person);

        Assert.Single(discussion.Messages);
        Assert.Equal("I think we should use strategy pattern", message.Body);
    }

    [Fact]
    public void Discussion_CannotAddMessage_WhenBacklogItemIsClosed()
    {
        var item = new BacklogItem("Feature X", "Description");
        var forum = new Forum(item);
        var person = CreatePerson();

        var discussion = forum.StartDiscussion("Topic", person);
        item.IsClosed = true;

        Assert.Throws<InvalidOperationException>(
            () => discussion.AddMessage("Too late", person));
    }

    [Fact]
    public void Forum_MultipleDiscussions_ShouldWork()
    {
        var item = new BacklogItem("Feature X", "Description");
        var forum = new Forum(item);

        forum.StartDiscussion("Topic 1", CreatePerson("Alice"));
        forum.StartDiscussion("Topic 2", CreatePerson("Bob"));
        forum.StartDiscussion("Topic 3", CreatePerson("Charlie"));

        Assert.Equal(3, forum.Discussions.Count);
    }

    [Fact]
    public void Forum_NewMessage_ShouldNotifySubscribers()
    {
        var item = new BacklogItem("Feature X", "Description");
        var forum = new Forum(item);
        var mockSubscriber = new Mock<ISubscriber>();
        forum.Subscribe(mockSubscriber.Object);

        var discussion = forum.StartDiscussion("Topic", CreatePerson());
        discussion.AddMessage("Hello team!", CreatePerson("Bob"));

        // StartDiscussion + AddMessage = 2 notifications
        mockSubscriber.Verify(s => s.Update(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void Forum_BacklogItemReopened_CanPostAgain()
    {
        var item = new BacklogItem("Feature X", "Description");
        var forum = new Forum(item);
        var person = CreatePerson();

        var discussion = forum.StartDiscussion("Topic", person);
        item.IsClosed = true;

        Assert.Throws<InvalidOperationException>(
            () => discussion.AddMessage("Closed", person));

        // Reopen
        item.IsClosed = false;
        var msg = discussion.AddMessage("Back open!", person);
        Assert.Equal("Back open!", msg.Body);
    }
}
