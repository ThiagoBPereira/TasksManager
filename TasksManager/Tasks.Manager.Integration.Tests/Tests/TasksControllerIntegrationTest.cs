using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using TasksManager.Application.ViewModels.Task;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]
    public class TasksControllerIntegrationTest : BaseAuthorizedRequestsTest
    {
        protected IEnumerable<TaskViewModelIndex> Tasks;

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Can_Create_Tasks()
        {
            var request = new RestRequest("users/{userName}/tasks", Method.POST);
            request.AddUrlSegment("userName", User.UserName);
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            var task = new TaskViewModelDetails
            {
                Title = "First Task",
                Description = "It can be null",
                IsCompleted = false
            };

            request.AddJsonBody(task);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Cant_Create_Tasks_Without_Title()
        {
            var request = new RestRequest("users/{userName}/tasks", Method.POST);
            request.AddUrlSegment("userName", User.UserName);
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            var task = new TaskViewModelDetails
            {
                Title = "",
                Description = "It can be null",
                IsCompleted = false
            };

            request.AddJsonBody(task);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Cant_Create_Another_User_Tasks()
        {
            var request = new RestRequest("users/{userName}/tasks", Method.POST);
            request.AddUrlSegment("userName", "AnotherUser");
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            var task = new TaskViewModelDetails
            {
                Title = "First Task",
                Description = "It can be null",
                IsCompleted = false
            };

            request.AddJsonBody(task);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void Everyone_Can_Get_Someone_Tasks()
        {
            var request = new RestRequest("users/{userName}/tasks", Method.GET);
            request.AddUrlSegment("userName", User.UserName);

            var response = RestCliente.Execute<List<TaskViewModelIndex>>(request);

            Tasks = new List<TaskViewModelIndex>(response.Data);

            Assert.IsNotNull(response.Data);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void Everyone_Can_Get_Details_Of_Someone_Tasks()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.GET);
            request.AddUrlSegment("userName", User.UserName);

            if (Tasks == null || !Tasks.Any())
                Everyone_Can_Get_Someone_Tasks();

            request.AddUrlSegment("taskId", Tasks?.FirstOrDefault()?.TaskId);

            var response = RestCliente.Execute<TaskViewModelDetails>(request);

            Assert.IsNotNull(response.Data);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Can_Update_Tasks()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.PUT);
            request.AddUrlSegment("userName", User.UserName);
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            if (Tasks == null || !Tasks.Any())
                Everyone_Can_Get_Someone_Tasks();

            request.AddUrlSegment("taskId", Tasks?.FirstOrDefault()?.TaskId);

            var task = new TaskViewModelDetails
            {
                Description = "It was modified",
                Title = "Second teste",
                IsCompleted = true
            };

            request.AddJsonBody(task);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }


        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Cant_Update_Tasks_Without_Autentication()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.PUT);
            request.AddUrlSegment("userName", User.UserName);

            if (Tasks == null || !Tasks.Any())
                Everyone_Can_Get_Someone_Tasks();

            request.AddUrlSegment("taskId", Tasks?.FirstOrDefault()?.TaskId);

            var task = new TaskViewModelDetails
            {
                Description = "It was modified",
                Title = "Second teste",
                IsCompleted = true
            };

            request.AddJsonBody(task);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Cant_Update_Another_User_Task()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.PUT);
            request.AddUrlSegment("userName", "AnotherUser");
            request.AddUrlSegment("taskId", "57b793aba2af1a24e03ff45f");

            var task = new TaskViewModelDetails
            {
                Description = "It was modified",
                Title = "Second teste",
                IsCompleted = true
            };

            request.AddJsonBody(task);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Cant_Delete_Tasks_Without_Autentication()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.DELETE);
            request.AddUrlSegment("userName", User.UserName);

            if (Tasks == null || !Tasks.Any())
                Everyone_Can_Get_Someone_Tasks();

            request.AddUrlSegment("taskId", Tasks?.FirstOrDefault()?.TaskId);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Cant_Delete_Another_User_Task()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.DELETE);
            request.AddUrlSegment("userName", "AnotherUser");
            request.AddUrlSegment("taskId", "57b793aba2af1a24e03ff45f");

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        [TestCategory("Integration/Tasks")]
        public void User_Can_Delete_Tasks()
        {
            var request = new RestRequest("users/{userName}/tasks/{taskId}", Method.DELETE);
            request.AddUrlSegment("userName", User.UserName);
            request.AddHeader("Authorization", "Bearer " + Token.access_token);

            if (Tasks == null || !Tasks.Any())
                Everyone_Can_Get_Someone_Tasks();

            request.AddUrlSegment("taskId", Tasks?.FirstOrDefault()?.TaskId);

            var response = RestCliente.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}