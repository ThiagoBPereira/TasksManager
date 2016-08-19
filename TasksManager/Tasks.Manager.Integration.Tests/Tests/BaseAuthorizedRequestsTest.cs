using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]
    public class BaseAuthorizedRequestsTest : BaseWebApiIntegrationTest
    {
        //Create user when not exist
        protected UserViewModelCreate User;

        //Get token
        protected TokenResponse Token;

        protected RestClient RestCliente;

        public BaseAuthorizedRequestsTest()
        {
            RestCliente = new RestClient(UrlApi);
            RestCliente.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

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

        [TestInitialize]
        public void GetToken()
        {

            var requestToken = new RestRequest("security/token", Method.POST);
            requestToken.AddParameter("grant_type", "password");
            requestToken.AddParameter("username", "token@token.com");
            requestToken.AddParameter("password", "123456ABC");

            var responseToken = RestCliente.Execute<TokenResponse>(requestToken);

            Token = responseToken.Data;

            //Getting token
            Assert.AreEqual(responseToken.StatusCode, HttpStatusCode.OK);
        }
    }
}