using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod]
        [TestCategory("Domain/Task")]
        public void Cant_Accept_Empty_title()
        {
            var taskUser = new Task
            {
                Title = "  "
            };

            Assert.IsFalse(taskUser.IsValid());
            Assert.IsNotNull(taskUser.ValidatorResult.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmptyError));
        }

        [TestMethod]
        [TestCategory("Domain/Task")]
        public void Can_Accept_title_populated()
        {
            var taskUser = new Task
            {
                Title = "Teste"
            };

            Assert.IsTrue(taskUser.IsValid());
        }

    }
}