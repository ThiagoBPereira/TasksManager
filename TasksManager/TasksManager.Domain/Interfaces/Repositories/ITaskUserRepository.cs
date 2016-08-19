using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Interfaces.Repositories
{
    public interface ITaskUserRepository : IBaseRepository<TaskUser>
    {
        TaskUser GetUserByEmailAndPassword(string email, string password);
        TaskUser GetUserByUserNameAndPassword(string id, string password);
        ValidatorResult ChangePassword(string userId, string oldPasswordHash, string passwordHash);
    }
}