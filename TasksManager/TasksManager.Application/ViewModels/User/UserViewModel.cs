using System.Configuration;
using TasksManager.Infra.Cc.Cryptographic;

namespace TasksManager.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}