namespace TasksManager.Infra.Cc.Validators
{
    public class ValidationError
    {
        public string Message { get; set; }
        public ErroKeyEnum ErrorKey { get; set; }

        public ValidationError(string message, ErroKeyEnum errorKey)
        {
            Message = message;
            ErrorKey = errorKey;
        }

        public ValidationError()
        {

        }
    }
}