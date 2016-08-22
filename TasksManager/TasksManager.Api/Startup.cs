using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using SimpleInjector.Integration.WebApi;
using TasksManager.Api.Authorize;
using TasksManager.Application.Interfaces;
using TasksManager.Infra.Cc.IoC;

namespace TasksManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            ConfigureWebApi(config);
            ResponseFormat(config);

            //Getting container
            var container = new SimpleContainer().Initialize();

            container.Verify();

            //Setting DependencyResolver
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);


            ConfigureOAuth(app, container.GetInstance<IUserApp>());

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public void ConfigureOAuth(IAppBuilder app, IUserApp iUserApp)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new AuthorizationServerProvider(iUserApp)
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public static void ResponseFormat(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            
            //Removing XML
            formatters.Remove(formatters.XmlFormatter);

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.Formatting = Formatting.Indented;
            json.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}