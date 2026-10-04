using TaskManager.Domain.Recurrence;

namespace TaskManager.Domain.Tests.Recurrence;

public class DailyRecurrenceTests
{
    [Fact]
    public void NextOccurrence_DefaultsToTheNextDay()
    {
        var rule = new DailyRecurrence();

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2026, 10, 6), next);
    }

    [Fact]
    public void NextOccurrence_AddsTheIntervalInDays()
    {
        var rule = new DailyRecurrence(interval: 3);

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2026, 10, 8), next);
    }

    [Fact]
    public void NextOccurrence_CrossesIntoTheNextYear()
    {
        var rule = new DailyRecurrence();

        var next = rule.NextOccurrence(new DateOnly(2026, 12, 31));

        Assert.Equal(new DateOnly(2027, 1, 1), next);
    }

    [Fact]
    public void NextOccurrence_WorksThroughABaseClassReference()
    {
        RecurrenceRule rule = new DailyRecurrence(interval: 2);

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2026, 10, 7), next);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NewRule_RejectsIntervalBelowOne(int interval)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DailyRecurrence(interval));
    }
}
