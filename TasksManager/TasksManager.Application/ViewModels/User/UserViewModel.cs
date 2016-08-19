using System.Configuration;
using TasksManager.Infra.Cc.Cryptographic;

namespace TasksManager.Application.ViewModels.User
{
    public class UserViewModelDetails
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}