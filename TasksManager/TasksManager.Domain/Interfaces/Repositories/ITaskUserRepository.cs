using TasksManager.Domain.Entities;

namespace TasksManager.Domain.Interfaces.Repositories
{
    public interface ITaskUserRepository : IBaseRepository<TaskUser>
    {
        TaskUser GetUserByEmailAndPassword(TaskUser taskUser);
    }
}