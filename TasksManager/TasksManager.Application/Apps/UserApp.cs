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
            var mongoUser = _taskUserService.GetUserByUserNameAndPassword(userViewModel.UserName, userViewModel.OldPassword);

            var validation = new ValidatorResult();

            if (mongoUser == null)
            {
                validation.AddError(new ValidationError("Username or password is incorrect", ErroKeyEnum.NotFound));
                return validation;
            }

            return _taskUserService.ChangePassword(userViewModel.UserName, userViewModel.NewPassword, userViewModel.NewPasswordConfirmation);
        }
    }
}