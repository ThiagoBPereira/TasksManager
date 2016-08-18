using TasksManager.Application.Interfaces;
using TasksManager.Application.ViewModels.User;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Application.Apps
{
    public class UserApp : IUserApp
    {
        private readonly ITaskUserRepository _taskUserRepository;

        public UserApp(ITaskUserRepository taskUserRepository)
        {
            _taskUserRepository = taskUserRepository;
        }

        public ValidatorResult Create(UserViewModel userViewModelViewModel)
        {
            var taskUser = new TaskUser
            {
                Email = userViewModelViewModel.Email,
                Password = userViewModelViewModel.Password,
                PasswordConfirmation = userViewModelViewModel.PasswordConfirmation
            };

            if (!taskUser.IsValid())
                return taskUser.ValidatorResult;

            return _taskUserRepository.CreateAsync(taskUser);
        }

        public UserViewModel GetUserByEmailAndPassword(UserViewModel userViewModel)
        {
            var taskUser = new TaskUser
            {
                Email = userViewModel.Email,
                Password = userViewModel.Password
            };

            //Generating hashPassword
            taskUser.GeneratePasswordHash();

            var mongoUser = _taskUserRepository.GetUserByEmailAndPassword(taskUser);

            userViewModel.Id = mongoUser.Id;

            return userViewModel;
        }
    }
}