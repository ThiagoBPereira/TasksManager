namespace TasksManager.Application.ViewModels.User
{
    public class UserViewModelCreate
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}