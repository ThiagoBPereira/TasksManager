using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]
    public class TokenAndAuthorizationTest : BaseWebApiIntegrationTest
    {
        [TestMethod]
        [TestCategory("Integration/Token")]
        public void User_Created_to_Get_A_Token()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("users", Method.POST);
            var newUser = new UserViewModelCreate
            {
                Email = "token@token.com",
                Password = "123456ABC",
                PasswordConfirmation = "123456ABC",
                UserName = "tokenUser"
            };

            request.AddJsonBody(newUser);

            var response2 = restCliente.Execute<ValidatorResult>(request);
            var errorData = response2.Data;

            //ErroKeyEnum.DuplicateKey is used to scape
            Assert.IsTrue(response2.StatusCode == HttpStatusCode.OK || errorData.Errors.FirstOrDefault(i => i.ErrorKey == ErroKeyEnum.DuplicateKey) != null);
        }

        [TestMethod]
        [TestCategory("Integration/Token")]
        public void Generate_token_with_correct_password()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "token@token.com");
            request.AddParameter("password", "123456ABC");

            var response = restCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        [TestCategory("Integration/Token")]
        public void Generate_token_with_incorrect_password()
        {
            var restCliente = new RestClient(UrlApi);
            restCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            var request = new RestRequest("security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "test7e@teste.com");
            request.AddParameter("password", "aaaaaaaaaaaaaaa");

            var response = restCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}