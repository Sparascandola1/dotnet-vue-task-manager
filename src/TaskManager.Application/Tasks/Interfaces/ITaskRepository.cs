using TaskManager.Domain.Tasks.Entities;

namespace TaskManager.Application.Tasks.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<TaskItem>> ListAsync();

    Task AddAsync(TaskItem task);

    Task UpdateAsync(TaskItem task);

    Task RemoveAsync(TaskItem task);
}
