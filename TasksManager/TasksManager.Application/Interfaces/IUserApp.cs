using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Application.Interfaces
{
    public interface IUserApp
    {
        ValidatorResult Create(UserViewModelCreate userViewModel);
        UserViewModelDetails GetUserByEmailAndPassword(string email, string password);
        ValidatorResult ChangePassword(UserChangePasswordViewModel userViewModel);
    }
}