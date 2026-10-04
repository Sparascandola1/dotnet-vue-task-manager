namespace TaskManager.Domain.Recurrence;

public sealed class DailyRecurrence : RecurrenceRule
{
    public DailyRecurrence(int interval = 1)
        : base(interval)
    {
    }

    public override DateOnly NextOccurrence(DateOnly from)
    {
        return from.AddDays(Interval);
    }
}
