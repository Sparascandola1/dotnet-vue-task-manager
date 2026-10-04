namespace TaskManager.Domain;

public sealed class MonthlyRecurrence : RecurrenceRule
{
    public MonthlyRecurrence(int interval = 1)
        : base(interval)
    {
    }

    public override DateOnly NextOccurrence(DateOnly from)
    {
        return from.AddMonths(Interval);
    }
}
