namespace TaskManager.Domain.Tests;

public class MonthlyRecurrenceTests
{
    [Fact]
    public void NextOccurrence_DefaultsToTheSameDayNextMonth()
    {
        var rule = new MonthlyRecurrence();

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2026, 11, 5), next);
    }

    [Fact]
    public void NextOccurrence_AddsTheIntervalInMonthsAcrossAYearBoundary()
    {
        var rule = new MonthlyRecurrence(interval: 3);

        var next = rule.NextOccurrence(new DateOnly(2026, 10, 5));

        Assert.Equal(new DateOnly(2027, 1, 5), next);
    }

    [Fact]
    public void NextOccurrence_UsesLastDayWhenNextMonthIsShorter()
    {
        var rule = new MonthlyRecurrence();

        var next = rule.NextOccurrence(new DateOnly(2027, 1, 31));

        Assert.Equal(new DateOnly(2027, 2, 28), next);
    }

    [Fact]
    public void NextOccurrence_UsesTheTwentyNinthOfFebruaryInALeapYear()
    {
        var rule = new MonthlyRecurrence();

        var next = rule.NextOccurrence(new DateOnly(2028, 1, 31));

        Assert.Equal(new DateOnly(2028, 2, 29), next);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NewRule_RejectsIntervalBelowOne(int interval)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonthlyRecurrence(interval));
    }
}
