using System.Collections.Generic;
using System.Linq;

namespace TasksManager.Infra.Cc.Validators
{
    public class ValidatorResult
    {
        public bool IsSuccess => !Errors.Any();

        public IList<ValidationError> Errors { get; set; }

        public ValidatorResult()
        {
            Errors = new List<ValidationError>();
        }

        public void AddError(ValidationError error)
        {
            Errors.Add(error);
        }
    }
}