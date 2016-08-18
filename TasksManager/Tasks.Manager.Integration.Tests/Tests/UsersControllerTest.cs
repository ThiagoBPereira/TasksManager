using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]

    public class UsersControllerIntegrationTest : BaseWebApiIntegrationTest
    {
        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Blank_Password()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModel
            {
                Email = "first@teste.com",
                Password = ""
            };

            request.AddJsonBody(newUser);

            var response = restCliente.Execute<ValidatorResult>(request);
            var data = response.Data;

            Assert.IsFalse(data.IsSuccess);
            Assert.IsNotNull(data.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmptyPassword));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Password_Less_Than_6_characters()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModel
            {
                Email = "first@teste.com",
                Password = "12345"
            };

            request.AddJsonBody(newUser);

            var response = restCliente.Execute<ValidatorResult>(request);
            var data = response.Data;

            Assert.IsFalse(data.IsSuccess);
            Assert.IsNotNull(data.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.SmallPassword));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Invalid_Email()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModel
            {
                Email = "first@.com",
                Password = "123456"
            };

            request.AddJsonBody(newUser);

            var response = restCliente.Execute<ValidatorResult>(request);
            var data = response.Data;

            Assert.IsFalse(data.IsSuccess);
            Assert.IsNotNull(data.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmailNotValid));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Different_Password()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModel
            {
                Email = "first@teste.com",
                Password = "123456",
                PasswordConfirmation = "123452"
            };

            request.AddJsonBody(newUser);

            var response2 = restCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;

            Assert.IsFalse(errorData.IsSuccess);
            Assert.IsNotNull(errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DontMatchPassword));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Can_Be_Created()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModel
            {
                Email = "first@teste.com",
                Password = "123456",
                PasswordConfirmation = "123456"
            };

            request.AddJsonBody(newUser);

            var response2 = restCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;

            //ErroKeyEnum.DuplicateKey is used to scape
            Assert.IsTrue(errorData.IsSuccess || errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DuplicateKey) != null);
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Same_Email()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModel
            {
                Email = "first@teste.com",
                Password = "123456",
                PasswordConfirmation = "123456"
            };

            request.AddJsonBody(newUser);

            //Creating one, to have error when create the second
            restCliente.Execute<ValidatorResult>(request);

            var response2 = restCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;


            Assert.IsFalse(errorData.IsSuccess);
            Assert.IsNotNull(errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DuplicateKey));
        }
    }
}
