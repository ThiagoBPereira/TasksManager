namespace TasksManager.Application.ViewModels.User
{
    public class UserChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}