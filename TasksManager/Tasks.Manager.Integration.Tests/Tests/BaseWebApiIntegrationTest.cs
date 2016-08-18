using System;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TasksManager.Api;

namespace Tasks.Manager.Integration.Tests.Tests
{
    [TestClass]
    public class BaseWebApiIntegrationTest
    {
        private static IDisposable _webApp;
        protected const string UrlBase = "http://localhost:2735";
        protected const string UrlApi = UrlBase + "/api";

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            _webApp = WebApp.Start<Startup>(UrlBase);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            _webApp.Dispose();
        }
    }
}