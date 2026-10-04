using TaskManager.Domain.Recurrence;
using TaskManager.Domain.Tasks.Enums;

namespace TaskManager.Domain.Tasks.Entities;

public class TaskItem
{
    public const int MaxTitleLength = 200;

    public TaskItem(string title, Priority priority = Priority.Medium)
    {
        if (!Enum.IsDefined(priority))
        {
            throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unknown priority.");
        }

        Id = Guid.NewGuid();
        Title = ValidateTitle(title);
        Priority = priority;
        Status = TaskItemStatus.Open;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public TaskItemStatus Status { get; private set; }

    public Priority Priority { get; private set; }

    public DateTime CreatedAt { get; }

    public DateTime? CompletedAt { get; private set; }

    public DateOnly? DueDate { get; private set; }

    public RecurrenceRule? Recurrence { get; private set; }

    public void Rename(string title)
    {
        Title = ValidateTitle(title);
    }

    public void Complete()
    {
        if (Status == TaskItemStatus.Completed)
        {
            throw new InvalidOperationException("This task has already been completed.");
        }

        Status = TaskItemStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Reopen()
    {
        if (Status == TaskItemStatus.Open)
        {
            throw new InvalidOperationException("This task is already open.");
        }

        Status = TaskItemStatus.Open;
        CompletedAt = null;
    }

    public void SetDueDate(DateOnly? dueDate)
    {
        if (dueDate is null && Recurrence is not null)
        {
            throw new InvalidOperationException("A recurring task must have a due date.");
        }

        DueDate = dueDate;
    }

    public void SetRecurrence(RecurrenceRule? recurrence)
    {
        if (recurrence is not null && DueDate is null)
        {
            throw new InvalidOperationException("A task needs a due date before it can recur.");
        }

        Recurrence = recurrence;
    }

    public TaskItem CreateNextOccurrence()
    {
        if (Recurrence is null || DueDate is null)
        {
            throw new InvalidOperationException("This task does not recur.");
        }

        var next = new TaskItem(Title, Priority);
        next.DueDate = Recurrence.NextOccurrence(DueDate.Value);
        next.Recurrence = Recurrence;

        return next;
    }

    private static string ValidateTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        var trimmed = title.Trim();
        if (trimmed.Length > MaxTitleLength)
        {
            throw new ArgumentException($"Title cannot be longer than {MaxTitleLength} characters.", nameof(title));
        }

        return trimmed;
    }
}
