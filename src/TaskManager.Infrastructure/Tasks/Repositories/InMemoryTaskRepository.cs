using System.Collections.Concurrent;
using TaskManager.Application.Tasks.Interfaces;
using TaskManager.Domain.Tasks.Entities;

namespace TaskManager.Infrastructure.Tasks.Repositories;

public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        _tasks.TryGetValue(id, out var task);

        return Task.FromResult(task);
    }

    public Task<IReadOnlyList<TaskItem>> ListAsync()
    {
        IReadOnlyList<TaskItem> tasks = _tasks.Values
            .OrderBy(task => task.CreatedAt)
            .ToList();

        return Task.FromResult(tasks);
    }

    public Task AddAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        _tasks[task.Id] = task;

        return Task.CompletedTask;
    }

    public Task UpdateAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        _tasks[task.Id] = task;

        return Task.CompletedTask;
    }

    public Task RemoveAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        _tasks.TryRemove(task.Id, out _);

        return Task.CompletedTask;
    }
}
