using TaskManager.Domain.Recurrence;
using TaskManager.Domain.Tasks.Entities;
using TaskManager.Domain.Tasks.Enums;

namespace TaskManager.Domain.Tests.Tasks;

public class TaskItemRecurrenceTests
{
    private static readonly DateOnly DueDate = new(2026, 10, 5);

    [Fact]
    public void NewTask_HasNoDueDateOrRecurrence()
    {
        var task = new TaskItem("Pay rent");

        Assert.Null(task.DueDate);
        Assert.Null(task.Recurrence);
    }

    [Fact]
    public void SetDueDate_SetsAndClearsTheDueDate()
    {
        var task = new TaskItem("Pay rent");

        task.SetDueDate(DueDate);
        Assert.Equal(DueDate, task.DueDate);

        task.SetDueDate(null);
        Assert.Null(task.DueDate);
    }

    [Fact]
    public void SetRecurrence_SetsAndClearsTheRule()
    {
        var task = new TaskItem("Pay rent");
        task.SetDueDate(DueDate);
        var rule = new MonthlyRecurrence();

        task.SetRecurrence(rule);
        Assert.Same(rule, task.Recurrence);

        task.SetRecurrence(null);
        Assert.Null(task.Recurrence);
    }

    [Fact]
    public void SetRecurrence_RejectsTaskWithoutDueDate()
    {
        var task = new TaskItem("Pay rent");

        var exception = Assert.Throws<InvalidOperationException>(() => task.SetRecurrence(new MonthlyRecurrence()));

        Assert.Equal("A task needs a due date before it can recur.", exception.Message);
        Assert.Null(task.Recurrence);
    }

    [Fact]
    public void SetDueDate_RejectsClearingTheDueDateOfRecurringTask()
    {
        var task = RecurringTask(new MonthlyRecurrence());

        var exception = Assert.Throws<InvalidOperationException>(() => task.SetDueDate(null));

        Assert.Equal("A recurring task must have a due date.", exception.Message);
        Assert.Equal(DueDate, task.DueDate);
    }

    [Fact]
    public void CreateNextOccurrence_RejectsTaskThatDoesNotRecur()
    {
        var task = new TaskItem("Pay rent");
        task.SetDueDate(DueDate);

        var exception = Assert.Throws<InvalidOperationException>(() => task.CreateNextOccurrence());

        Assert.Equal("This task does not recur.", exception.Message);
    }

    [Fact]
    public void CreateNextOccurrence_ReturnsNewOpenTaskWithSameDetails()
    {
        var rule = new MonthlyRecurrence();
        var task = new TaskItem("Pay rent", Priority.High);
        task.SetDueDate(DueDate);
        task.SetRecurrence(rule);
        task.Complete();

        var next = task.CreateNextOccurrence();

        Assert.NotEqual(task.Id, next.Id);
        Assert.Equal("Pay rent", next.Title);
        Assert.Equal(Priority.High, next.Priority);
        Assert.Equal(TaskItemStatus.Open, next.Status);
        Assert.Null(next.CompletedAt);
        Assert.Same(rule, next.Recurrence);
    }

    [Fact]
    public void CreateNextOccurrence_LeavesTheOriginalTaskUnchanged()
    {
        var task = RecurringTask(new WeeklyRecurrence());
        task.Complete();

        task.CreateNextOccurrence();

        Assert.Equal(TaskItemStatus.Completed, task.Status);
        Assert.Equal(DueDate, task.DueDate);
    }

    [Fact]
    public void CreateNextOccurrence_TakesTheDueDateFromADailyRule()
    {
        var next = RecurringTask(new DailyRecurrence()).CreateNextOccurrence();

        Assert.Equal(new DateOnly(2026, 10, 6), next.DueDate);
    }

    [Fact]
    public void CreateNextOccurrence_TakesTheDueDateFromAWeeklyRule()
    {
        var next = RecurringTask(new WeeklyRecurrence()).CreateNextOccurrence();

        Assert.Equal(new DateOnly(2026, 10, 12), next.DueDate);
    }

    [Fact]
    public void CreateNextOccurrence_TakesTheDueDateFromAMonthlyRule()
    {
        var next = RecurringTask(new MonthlyRecurrence()).CreateNextOccurrence();

        Assert.Equal(new DateOnly(2026, 11, 5), next.DueDate);
    }

    private static TaskItem RecurringTask(RecurrenceRule rule)
    {
        var task = new TaskItem("Pay rent");
        task.SetDueDate(DueDate);
        task.SetRecurrence(rule);
        return task;
    }
}
