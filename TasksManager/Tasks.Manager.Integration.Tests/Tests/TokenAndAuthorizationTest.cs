using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]
    public class TokenAndAuthorizationTest : BaseAuthorizedRequestsTest
    {
        [TestMethod]
        [TestCategory("Integration/Token")]
        public void Generate_token_with_correct_password()
        {
            var request = new RestRequest("security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "token@token.com");
            request.AddParameter("password", "123456ABC");

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

        }

        [TestMethod]
        [TestCategory("Integration/Token")]
        public void Generate_token_with_incorrect_password()
        {
            var request = new RestRequest("security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "test7e@teste.com");
            request.AddParameter("password", "aaaaaaaaaaaaaaa");

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}