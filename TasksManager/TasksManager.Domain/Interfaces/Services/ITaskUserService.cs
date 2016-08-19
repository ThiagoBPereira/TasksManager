using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Interfaces.Services
{
    public interface ITaskUserService
    {
        TaskUser GetUserByEmailAndPassword(string email, string password);
        TaskUser GetUserByUserNameAndPassword(string userName, string password);
        ValidatorResult CreateAsync(TaskUser taskUser);
        ValidatorResult ChangePassword(string id, string oldPassword, string newPassword, string newPasswordConfirmation);
    }
}