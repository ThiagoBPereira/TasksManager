using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Application.Interfaces
{
    public interface IUserApp
    {
        ValidatorResult Create(UserViewModel userViewModel);
        UserViewModel GetUserByEmailAndPassword(UserViewModel userViewModel);
    }
}