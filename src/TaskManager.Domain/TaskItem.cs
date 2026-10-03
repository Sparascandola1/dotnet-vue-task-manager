namespace TaskManager.Domain;

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
