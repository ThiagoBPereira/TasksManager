using System.Configuration;
using TasksManager.Infra.Cc.Cryptographic;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.ValueObjects
{
    public static class PasswordHelper
    {
        public static ValidatorResult IsPasswordValid(string password, string passwordConfirmation)
        {
            var validation = new ValidatorResult();
            if (string.IsNullOrEmpty(password))
            {
                validation.AddError(new ValidationError("Please enter your password", ErroKeyEnum.EmptyError));
            }
            else if (password.Length < 6)
            {
                validation.AddError(new ValidationError("To be valid your password have to contain at least 6 characters", ErroKeyEnum.SmallLenghtError));
            }
            else if (!password.Equals(passwordConfirmation))
            {
                validation.AddError(new ValidationError("Password and Password confirmation do not match", ErroKeyEnum.DontMatchPassword));
            }
            return validation;
        }

        public static string GenerateHash(string password)
        {
            return Encrypt.GenerateSha256Hash(password, ConfigurationManager.AppSettings["EncryptSalt"]);
        }
    }
}