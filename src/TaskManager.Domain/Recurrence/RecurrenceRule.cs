namespace TaskManager.Domain.Recurrence;

public abstract class RecurrenceRule
{
    protected RecurrenceRule(int interval)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(interval, 1);

        Interval = interval;
    }

    public int Interval { get; }

    public abstract DateOnly NextOccurrence(DateOnly from);
}
