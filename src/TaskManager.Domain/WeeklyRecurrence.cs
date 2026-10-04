namespace TaskManager.Domain;

public sealed class WeeklyRecurrence : RecurrenceRule
{
    private const int DaysPerWeek = 7;

    public WeeklyRecurrence(int interval = 1)
        : base(interval)
    {
    }

    public override DateOnly NextOccurrence(DateOnly from)
    {
        return from.AddDays(Interval * DaysPerWeek);
    }
}
