using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Tests
{
    [TestClass]
    public class TaskUserTests
    {
        [TestMethod]
        [TestCategory("Integration/User/Domain")]
        public void Can_Accept_Valid_Emails()
        {
            var taskUser = new TaskUser
            {
                Email = "thiago.bpereira@gmail.com",
                Password = "123123",
                PasswordConfirmation = "123123"
            };

            Assert.IsTrue(taskUser.IsValid());
            Assert.IsNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmailNotValid));

            taskUser.Email = "thiago.bpereira@al.hotmail.com";

            Assert.IsTrue(taskUser.IsValid());
            Assert.IsNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmailNotValid));
        }

        [TestMethod]
        [TestCategory("Integration/User/Domain")]
        public void Cant_Accept_Invalid_Emails()
        {
            var taskUser = new TaskUser
            {
                Email = "",
                Password = "123123",
                PasswordConfirmation = "123123"
            };

            Assert.IsFalse(taskUser.IsValid());
            Assert.IsNotNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmailNotValid));

            taskUser.Email = "tteste";

            Assert.IsFalse(taskUser.IsValid());
            Assert.IsNotNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmailNotValid));
        }

        [TestMethod]
        [TestCategory("Integration/User/Domain")]
        public void Cant_Accept_Different_Passwords()
        {
            var taskUser = new TaskUser
            {
                Email = "thiago.bpereira@gmail.com",
                Password = "123124",
                PasswordConfirmation = "123123"
            };

            Assert.IsFalse(taskUser.IsValid());
            Assert.IsNotNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DontMatchPassword));
        }

        [TestMethod]
        [TestCategory("Integration/User/Domain")]
        public void Cant_Accept_Empty_Passwords()
        {
            var taskUser = new TaskUser
            {
                Email = "thiago.bpereira@gmail.com",
                Password = "",
                PasswordConfirmation = "123123"
            };

            Assert.IsFalse(taskUser.IsValid());
            Assert.IsNotNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmptyPassword));
        }

        [TestMethod]
        [TestCategory("Integration/User/Domain")]
        public void Cant_Accept_Small_Passwords()
        {
            var taskUser = new TaskUser
            {
                Email = "thiago.bpereira@gmail.com",
                Password = "123",
                PasswordConfirmation = "123"
            };

            Assert.IsFalse(taskUser.IsValid());
            Assert.IsNotNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.SmallPassword));
        }
    }
}
