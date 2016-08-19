using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Domain.Interfaces.Services;
using TasksManager.Domain.ValueObjects;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Services
{
    public class TaskUserService : ITaskUserService
    {
        private readonly ITaskUserRepository _taskUserRepository;

        public TaskUserService(ITaskUserRepository taskUserRepository)
        {
            _taskUserRepository = taskUserRepository;
        }

        public TaskUser GetUserByEmailAndPassword(string email, string password)
        {
            var passwordHash = PasswordHelper.GenerateHash(password);

            return _taskUserRepository.GetUserByEmailAndPassword(email, passwordHash);
        }

        public TaskUser GetUserByUserNameAndPassword(string username, string password)
        {
            //create hash
            var passwordHash = PasswordHelper.GenerateHash(password);

            return _taskUserRepository.GetUserByUserNameAndPassword(username, passwordHash);
        }

        public ValidatorResult CreateAsync(TaskUser taskUser)
        {
            if (taskUser.IsValid())
            {
                return _taskUserRepository.CreateAsync(taskUser);
            }

            return taskUser.ValidatorResult;
        }

        public ValidatorResult ChangePassword(string id, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            var validation = PasswordHelper.IsPasswordValid(newPassword, newPasswordConfirmation);
            if (validation.IsSuccess)
            {
                var passwordHash = PasswordHelper.GenerateHash(newPassword);
                var oldPasswordHash = PasswordHelper.GenerateHash(oldPassword);

                return _taskUserRepository.ChangePassword(id, oldPasswordHash, passwordHash);
            }
            return validation;
        }
    }
}