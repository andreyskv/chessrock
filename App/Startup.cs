using App.Plugins;
using App.Models;
using App.Identity;
using App.Common;
using App.ViewModels;
using AutoMapper;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Core;
using System.Web.Routing;
using System.Web.Http;
using System;
using App.Repository;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
//[assembly: OwinStartup(typeof(App.Startup))]

namespace App
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {              

            //Configure AutoMapper (http://automapper.codeplex.com/)
            Mapper.Initialize(ConfigureMapper);

            //Configure Bearer Authentication         
            OAuthOptions = new OAuthAuthorizationServerOptions();
            app.UseOAuthBearerTokens(OAuthOptions);

            //Configure AutoFac for DependencyResolver (http://autofac.org/)
            IContainer container = RegisterServices();            
            var resolver = new App.Common.AutoFacDependencyResolver(container);

            //Configure WebApi
            var config = new HttpConfiguration() { DependencyResolver = resolver };
            ConfigureWebApi(config);
            app.UseWebApi(config);
            
        }

        private  void ConfigureWebApi(HttpConfiguration config)
        {
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private IContainer RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Startup).Assembly).Where(t => t.Name.EndsWith("Controller")).AsSelf();

            builder.RegisterType<AppRepository>().As<IRepository>().As<IAsyncRepository>();            
            builder.RegisterType<LocalUserLoginProvider>().As<ILoginProvider>().SingleInstance();                      
            return builder.Build();
        }

        public static void ConfigureMapper(IConfiguration config)
        {
            config.CreateMap<TodoItem, TodoItemViewModel>().ReverseMap();
            config.CreateMap<TodoList, TodoListViewModel>().ReverseMap();
        }
        

    }
}