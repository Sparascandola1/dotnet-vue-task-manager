using TaskManager.Application.Tasks.Interfaces;
using TaskManager.Infrastructure.Tasks.Repositories;

namespace TaskManager.Infrastructure.Tests.Tasks;

public class InMemoryTaskRepositoryTests : TaskRepositoryTests
{
    protected override ITaskRepository CreateRepository()
    {
        return new InMemoryTaskRepository();
    }
}
