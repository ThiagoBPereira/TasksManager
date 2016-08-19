namespace TasksManager.Application.ViewModels.Request
{
    public class UserNameRequest
    {
        private string _userName;

        public string UserName
        {
            get { return _userName?.ToLower(); }
            set { _userName = value; }
        }
    }
}