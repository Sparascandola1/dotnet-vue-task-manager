using TaskManager.Domain.Tasks.Entities;
using TaskManager.Domain.Tasks.Enums;

namespace TaskManager.Domain.Tests.Tasks;

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

    [Fact]
    public void Rename_ChangesTitle()
    {
        var task = new TaskItem("Write unit tests");

        task.Rename("Write more unit tests");

        Assert.Equal("Write more unit tests", task.Title);
    }

    [Fact]
    public void Rename_TrimsTitle()
    {
        var task = new TaskItem("Write unit tests");

        task.Rename("  Write more unit tests  ");

        Assert.Equal("Write more unit tests", task.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_RejectsBlankTitleAndKeepsOldTitle(string? title)
    {
        var task = new TaskItem("Write unit tests");

        Assert.ThrowsAny<ArgumentException>(() => task.Rename(title!));
        Assert.Equal("Write unit tests", task.Title);
    }

    [Fact]
    public void Rename_RejectsTitleLongerThanMaximum()
    {
        var task = new TaskItem("Write unit tests");
        var title = new string('a', TaskItem.MaxTitleLength + 1);

        Assert.Throws<ArgumentException>(() => task.Rename(title));
        Assert.Equal("Write unit tests", task.Title);
    }

    [Fact]
    public void NewTask_HasNoCompletedTimestamp()
    {
        var task = new TaskItem("Write unit tests");

        Assert.Null(task.CompletedAt);
    }

    [Fact]
    public void Complete_MarksTaskCompletedAndRecordsWhen()
    {
        var task = new TaskItem("Write unit tests");
        var before = DateTime.UtcNow;

        task.Complete();

        Assert.Equal(TaskItemStatus.Completed, task.Status);
        Assert.NotNull(task.CompletedAt);
        Assert.InRange(task.CompletedAt.Value, before, DateTime.UtcNow);
    }

    [Fact]
    public void Complete_RejectsTaskThatIsAlreadyCompleted()
    {
        var task = new TaskItem("Write unit tests");
        task.Complete();
        var completedAt = task.CompletedAt;

        var exception = Assert.Throws<InvalidOperationException>(() => task.Complete());

        Assert.Equal("This task has already been completed.", exception.Message);
        Assert.Equal(completedAt, task.CompletedAt);
    }

    [Fact]
    public void Reopen_MarksTaskOpenAndClearsCompletedTimestamp()
    {
        var task = new TaskItem("Write unit tests");
        task.Complete();

        task.Reopen();

        Assert.Equal(TaskItemStatus.Open, task.Status);
        Assert.Null(task.CompletedAt);
    }

    [Fact]
    public void Reopen_RejectsTaskThatIsAlreadyOpen()
    {
        var task = new TaskItem("Write unit tests");

        var exception = Assert.Throws<InvalidOperationException>(() => task.Reopen());

        Assert.Equal("This task is already open.", exception.Message);
    }
}
