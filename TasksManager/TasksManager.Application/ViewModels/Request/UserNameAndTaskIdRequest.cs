namespace TasksManager.Application.ViewModels.Request
{
    public class UserNameAndTaskIdRequest : UserNameRequest
    {
        public string TaskId { get; set; }
    }
}