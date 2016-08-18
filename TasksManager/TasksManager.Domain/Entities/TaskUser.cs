using System.Configuration;
using System.Text.RegularExpressions;
using AspNet.Identity.MongoDB;
using MongoDB.Bson.Serialization.Attributes;
using TasksManager.Infra.Cc.Cryptographic;
using TasksManager.Infra.Cc.Validators;

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

            if (ValidatorResult.IsSuccess)
            {
                GeneratePasswordHash();
            }

            return ValidatorResult.IsSuccess;
        }

        private void VerifyEmail()
        {
            //Email have to be valid
            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
            var match = regex.Match(Email);

            if (!match.Success)
            {
                ValidatorResult.AddError(new ValidationError("Your email address is invalid. Please enter a valid address.", ErroKeyEnum.EmailNotValid));
            }
        }

        private void VerifyPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                ValidatorResult.AddError(new ValidationError("Please enter your password", ErroKeyEnum.EmptyPassword));
            }
            else if (Password.Length < 6)
            {
                ValidatorResult.AddError(new ValidationError("To be valid your password have to contain at least 6 characters", ErroKeyEnum.SmallPassword));
            }
            else if (!Password.Equals(PasswordConfirmation))
            {
                ValidatorResult.AddError(new ValidationError("Password and Password confirmation do not match", ErroKeyEnum.DontMatchPassword));
            }
        }

        public void GeneratePasswordHash()
        {
            PasswordHash = Encrypt.GenerateSha256Hash(Password, ConfigurationManager.AppSettings["EncryptSalt"]);
        }
    }
}