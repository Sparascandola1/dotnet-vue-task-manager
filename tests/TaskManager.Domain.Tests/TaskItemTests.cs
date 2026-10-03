namespace TaskManager.Domain.Tests;

public class TaskItemTests
{
    [Fact]
    public void NewTask_HasGivenTitleAndIsOpen()
    {
        var task = new TaskItem("Write unit tests");

        Assert.Equal("Write unit tests", task.Title);
        Assert.Equal(TaskItemStatus.Open, task.Status);
    }

    [Fact]
    public void NewTask_DefaultsToMediumPriority()
    {
        var task = new TaskItem("Write unit tests");

        Assert.Equal(Priority.Medium, task.Priority);
    }

    [Fact]
    public void NewTask_UsesGivenPriority()
    {
        var task = new TaskItem("Write unit tests", Priority.High);

        Assert.Equal(Priority.High, task.Priority);
    }

    [Fact]
    public void NewTasks_GetDifferentIds()
    {
        var first = new TaskItem("First");
        var second = new TaskItem("Second");

        Assert.NotEqual(Guid.Empty, first.Id);
        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public void NewTask_TrimsTitle()
    {
        var task = new TaskItem("  Write unit tests  ");

        Assert.Equal("Write unit tests", task.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NewTask_RejectsBlankTitle(string? title)
    {
        Assert.ThrowsAny<ArgumentException>(() => new TaskItem(title!));
    }

    [Fact]
    public void NewTask_RejectsTitleLongerThanMaximum()
    {
        var title = new string('a', TaskItem.MaxTitleLength + 1);

        Assert.Throws<ArgumentException>(() => new TaskItem(title));
    }

    [Fact]
    public void NewTask_RejectsUndefinedPriority()
    {
        var undefined = (Priority)42;

        Assert.Throws<ArgumentOutOfRangeException>(() => new TaskItem("Write unit tests", undefined));
    }
}
