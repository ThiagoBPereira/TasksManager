using TasksManager.Application.Interfaces;
using TasksManager.Application.ViewModels.User;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Services;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Application.Apps
{
    public class UserApp : IUserApp
    {
        private readonly ITaskUserService _taskUserService;

        public UserApp(ITaskUserService taskUserService)
        {
            _taskUserService = taskUserService;
        }

        public ValidatorResult Create(UserViewModelCreate userViewModelViewModel)
        {
            var taskUser = new TaskUser
            {
                Email = userViewModelViewModel.Email,
                Password = userViewModelViewModel.Password,
                PasswordConfirmation = userViewModelViewModel.PasswordConfirmation,
                UserName = userViewModelViewModel.UserName?.ToLower()
            };

            return _taskUserService.CreateAsync(taskUser);
        }

        public UserViewModelDetails GetUserByEmailAndPassword(string email, string password)
        {
            var taskUser = _taskUserService.GetUserByEmailAndPassword(email, password);

            //Mapper
            return new UserViewModelDetails
            {
                Email = taskUser.Email,
                UserName = taskUser.UserName,
                Id = taskUser.Id
            };
        }

        public ValidatorResult ChangePassword(UserChangePasswordViewModel userViewModel)
        {
            return _taskUserService.ChangePassword(userViewModel.UserName, userViewModel.OldPassword, userViewModel.NewPassword, userViewModel.NewPasswordConfirmation);
        }
    }
}