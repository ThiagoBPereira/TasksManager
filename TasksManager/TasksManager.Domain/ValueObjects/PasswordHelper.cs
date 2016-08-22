using System.Configuration;
using TasksManager.Infra.Cc.Cryptographic;
using TasksManager.Infra.Cc.Validators;
using TasksManager.Infra.IoC.Resources;

namespace TasksManager.Domain.ValueObjects
{
    public static class PasswordHelper
    {
        public static ValidatorResult IsPasswordValid(string password, string passwordConfirmation)
        {
            var validation = new ValidatorResult();
            if (string.IsNullOrEmpty(password))
            {
                validation.AddError(new ValidationError(string.Format(Resources.PleaseEnterYour, Resources.Password), ErroKeyEnum.EmptyError));
            }
            else if (password.Length < 6)
            {
                validation.AddError(new ValidationError(string.Format(Resources.ToBeValidYourHaveToContain, Resources.Password, "6"), ErroKeyEnum.SmallLenghtError));
            }
            else if (!password.Equals(passwordConfirmation))
            {
                validation.AddError(new ValidationError(string.Format(Resources.FieldAndField2DontMatch, Resources.Password, Resources.PasswordConfirmation), ErroKeyEnum.DontMatchPassword));
            }
            return validation;
        }

        public static string GenerateHash(string password)
        {
            return Encrypt.GenerateSha256Hash(password, ConfigurationManager.AppSettings["EncryptSalt"]);
        }
    }
}