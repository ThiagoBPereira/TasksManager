using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]
    public class UsersControllerIntegrationTest : BaseAuthorizedRequestsTest
    {
        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Blank_Password()
        {
            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModelCreate()
            {
                Email = "first@teste.com",
                Password = ""
            };

            request.AddJsonBody(newUser);

            var response = RestCliente.Execute<ValidatorResult>(request);
            var data = response.Data;

            Assert.IsFalse(data.IsSuccess);
            Assert.IsNotNull(data.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.EmptyError));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Password_Less_Than_6_characters()
        {
            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModelCreate
            {
                Email = "first@teste.com",
                Password = "12345"
            };

            request.AddJsonBody(newUser);

            var response = RestCliente.Execute<ValidatorResult>(request);
            var data = response.Data;

            Assert.IsFalse(data.IsSuccess);
            Assert.IsNotNull(data.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.SmallLenghtError));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Invalid_Email()
        {
            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModelCreate
            {
                Email = "first@.com",
                Password = "123456"
            };

            request.AddJsonBody(newUser);

            var response = RestCliente.Execute<ValidatorResult>(request);
            var data = response.Data;

            Assert.IsFalse(data.IsSuccess);
            Assert.IsNotNull(data.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.NotValid));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Different_Password()
        {
            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModelCreate
            {
                Email = "first@teste.com",
                Password = "123456",
                PasswordConfirmation = "123452"
            };

            request.AddJsonBody(newUser);

            var response2 = RestCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;

            Assert.IsFalse(errorData.IsSuccess);
            Assert.IsNotNull(errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DontMatchPassword));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Can_Be_Created()
        {
            var request = new RestRequest("users", Method.POST);

            User = new UserViewModelCreate
            {
                Email = "token@token.com",
                Password = "123456ABC",
                PasswordConfirmation = "123456ABC",
                UserName = "tokenUser"
            };

            request.AddJsonBody(User);

            var response2 = RestCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;

            //ErroKeyEnum.DuplicateKey is used to scape
            Assert.IsTrue(response2.StatusCode == HttpStatusCode.OK || errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DuplicateKey) != null);
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Be_Created_With_Same_Email()
        {
            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModelCreate
            {
                Email = "token@token.com",
                Password = "123456",
                PasswordConfirmation = "123456",
                UserName = "first"
            };

            request.AddJsonBody(newUser);

            var response2 = RestCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;


            Assert.IsFalse(errorData.IsSuccess);
            Assert.IsNotNull(errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DuplicateKey));
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Change_Password_Without_Authnetication()
        {
            var request = new RestRequest("users/{userName}/ChangePassword", Method.PUT);
            request.AddUrlSegment("userName", "tokenUser");

            var changePassword = new UserChangePasswordViewModel
            {
                OldPassword = "123456ABC",
                NewPassword = "123456ABC",
                NewPasswordConfirmation = "123456ABC"
            };

            request.AddJsonBody(changePassword);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Can_Change_Password_With_Authentication()
        {
            var request = new RestRequest("users/{userName}/ChangePassword", Method.PUT);
            request.AddUrlSegment("userName", "tokenUser");
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            var changePassword = new UserChangePasswordViewModel
            {
                OldPassword = "123456ABC",
                NewPassword = "123456ABC",
                NewPasswordConfirmation = "123456ABC"
            };

            request.AddJsonBody(changePassword);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory("Integration/User")]
        public void User_Cant_Change_Password_of_Another_User_With_Authentication()
        {
            var request = new RestRequest("users/{userName}/ChangePassword", Method.PUT);
            request.AddUrlSegment("userName", "first");
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            var changePassword = new UserChangePasswordViewModel
            {
                OldPassword = "123456ABC",
                NewPassword = "123456ABC",
                NewPasswordConfirmation = "123456ABC"
            };

            request.AddJsonBody(changePassword);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }
    }
}
