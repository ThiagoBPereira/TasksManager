using System.Text.RegularExpressions;
using AspNet.Identity.MongoDB;
using MongoDB.Bson.Serialization.Attributes;
using TasksManager.Domain.ValueObjects;
using TasksManager.Infra.Cc.Validators;
using TasksManager.Infra.IoC.Resources;

namespace TasksManager.Domain.Entities
{
    public class TaskUser : IdentityUser
    {

        [BsonIgnore]
        public ValidatorResult ValidatorResult { get; set; }

        [BsonIgnore]
        public string Password { get; set; }

        [BsonIgnore]
        public string PasswordConfirmation { get; set; }

        public bool IsValid()
        {
            ValidatorResult = new ValidatorResult();

            VerifyEmail();

            VerifyPassword();

            VerifyUsername();

            if (ValidatorResult.IsSuccess)
            {
                PasswordHash = PasswordHelper.GenerateHash(Password);
            }

            return ValidatorResult.IsSuccess;
        }

        private void VerifyUsername()
        {
            var regex = new Regex(@"[a-zA-Z-0-9]+ ", RegexOptions.IgnoreCase);

            if (string.IsNullOrEmpty(UserName))
            {
                ValidatorResult.AddError(new ValidationError(string.Format(Resources.PleaseEnterYour, Resources.Username), ErroKeyEnum.EmptyError));
            }
            else if (UserName.Length < 4)
            {
                ValidatorResult.AddError(new ValidationError(string.Format(Resources.ToBeValidYourHaveToContain, Resources.Username, "4"), ErroKeyEnum.SmallLenghtError));
            }
            else if (regex.Match(Email).Success)
            {
                ValidatorResult.AddError(new ValidationError(string.Format(Resources.ToBeValidYourHaveToContainOnly, Resources.Username, Resources.LettersAndNumbers), ErroKeyEnum.NotValid));
            }
        }

        private void VerifyEmail()
        {
            if (!string.IsNullOrEmpty(Email))
            {
                //Email have to be valid
                var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
                var match = regex.Match(Email);

                if (!match.Success)
                {
                    ValidatorResult.AddError(new ValidationError(string.Format(Resources.PleaseEnterAValid, Resources.EmailAddress), ErroKeyEnum.NotValid));
                }
            }
            else
            {
                ValidatorResult.AddError(new ValidationError(string.Format(Resources.PleaseEnterYour, Resources.EmailAddress), ErroKeyEnum.EmptyError));
            }
        }

        private void VerifyPassword()
        {
            ValidatorResult.AddError(PasswordHelper.IsPasswordValid(Password, PasswordConfirmation));
        }


    }
}