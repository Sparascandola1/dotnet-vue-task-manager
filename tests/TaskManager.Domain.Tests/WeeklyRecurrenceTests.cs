namespace TaskManager.Domain.Tests;

public class WeeklyRecurrenceTests
{
    [Fact]
    public void NextOccurrence_DefaultsToOneWeekLater()
    {
        var rule = new WeeklyRecurrence();

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2026, 10, 12), next);
    }

    [Fact]
    public void NextOccurrence_AddsTheIntervalInWeeks()
    {
        var rule = new WeeklyRecurrence(interval: 2);

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2026, 10, 19), next);
    }

    [Fact]
    public void NextOccurrence_FallsOnTheSameDayOfTheWeek()
    {
        var rule = new WeeklyRecurrence(interval: 3);
        var from = new DateOnly(2026, 10, 5);

        var next = rule.NextOccurrence(from);

        Assert.Equal(from.DayOfWeek, next.DayOfWeek);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NewRule_RejectsIntervalBelowOne(int interval)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new WeeklyRecurrence(interval));
    }
}
