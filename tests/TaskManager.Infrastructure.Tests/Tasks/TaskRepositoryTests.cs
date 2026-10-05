using TaskManager.Application.Tasks.Interfaces;
using TaskManager.Domain.Tasks.Entities;
using TaskManager.Domain.Tasks.Enums;

namespace TaskManager.Infrastructure.Tests.Tasks;

public abstract class TaskRepositoryTests
{
    protected abstract ITaskRepository CreateRepository();

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenTaskDoesNotExist()
    {
        var repository = CreateRepository();

        var found = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(found);
    }

    [Fact]
    public async Task AddAsync_StoresTaskSoItCanBeFoundById()
    {
        var repository = CreateRepository();
        var task = new TaskItem("Write unit tests", Priority.High);

        await repository.AddAsync(task);
        var found = await repository.GetByIdAsync(task.Id);

        Assert.NotNull(found);
        Assert.Equal(task.Id, found.Id);
        Assert.Equal("Write unit tests", found.Title);
        Assert.Equal(Priority.High, found.Priority);
    }

    [Fact]
    public async Task ListAsync_ReturnsEmptyListWhenNothingIsStored()
    {
        var repository = CreateRepository();

        var tasks = await repository.ListAsync();

        Assert.Empty(tasks);
    }

    [Fact]
    public async Task ListAsync_ReturnsEveryStoredTask()
    {
        var repository = CreateRepository();
        var first = new TaskItem("First");
        var second = new TaskItem("Second");
        await repository.AddAsync(first);
        await repository.AddAsync(second);

        var tasks = await repository.ListAsync();

        Assert.Equal(2, tasks.Count);
        Assert.Contains(tasks, t => t.Id == first.Id);
        Assert.Contains(tasks, t => t.Id == second.Id);
    }

    [Fact]
    public async Task UpdateAsync_StoresChangesToTask()
    {
        var repository = CreateRepository();
        var task = new TaskItem("Write unit tests");
        await repository.AddAsync(task);

        task.Rename("Write more unit tests");
        task.Complete();
        await repository.UpdateAsync(task);
        var found = await repository.GetByIdAsync(task.Id);

        Assert.NotNull(found);
        Assert.Equal("Write more unit tests", found.Title);
        Assert.Equal(TaskItemStatus.Completed, found.Status);
    }

    [Fact]
    public async Task RemoveAsync_RemovesOnlyTheGivenTask()
    {
        var repository = CreateRepository();
        var removed = new TaskItem("Remove me");
        var kept = new TaskItem("Keep me");
        await repository.AddAsync(removed);
        await repository.AddAsync(kept);

        await repository.RemoveAsync(removed);

        Assert.Null(await repository.GetByIdAsync(removed.Id));
        Assert.NotNull(await repository.GetByIdAsync(kept.Id));
    }
}
